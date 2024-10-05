import { LexicalComposer } from "@lexical/react/LexicalComposer";
import { ContentEditable } from "@lexical/react/LexicalContentEditable";
import { LexicalErrorBoundary } from "@lexical/react/LexicalErrorBoundary";
import { RichTextPlugin } from "@lexical/react/LexicalRichTextPlugin";
import classes from "./Editor.module.css";
import ToolbarPlugin from "./Plugins/ToolbarPlugins/ToolbarPlugin";
import { HeadingNode } from "@lexical/rich-text";
import { ListNode, ListItemNode } from "@lexical/list";
import { ListPlugin } from "@lexical/react/LexicalListPlugin";
import { CollaborationPlugin } from "@lexical/react/LexicalCollaborationPlugin";
import { Provider } from "@lexical/yjs";
import * as Y from "yjs";
import { WebsocketProvider } from "y-websocket";
import { getRandomUserProfile } from "../getRandomUserProfile";
import { useCallback, useEffect, useState } from "react";
import { theme } from "./Theming/EditorThemes";
import { ActiveUserProfile } from "./Interfaces/ActiveUserProfile";
import "./Theming/theming.css";

function getDocFromMap(id: string, yjsDocMap: Map<string, Y.Doc>): Y.Doc {
  let doc = yjsDocMap.get(id);

  if (doc === undefined) {
    doc = new Y.Doc();
    yjsDocMap.set(id, doc);
  } else {
    doc.load();
  }

  return doc;
}

const initialConfig = {
  namespace: "MyEditor",
  theme: theme,
  onError: (error: Error) => console.log(error),
  nodes: [HeadingNode, ListNode, ListItemNode],
  editorState: null,
};

export default function Editor() {
  const [userProfile] = useState(() => getRandomUserProfile());
  const [activeUsers, setActiveUsers] = useState<ActiveUserProfile[]>([]);
  const [yjsProvider, setYjsProvider] = useState<null | Provider>(null);
  const [providerName] = useState("websocket");

  const handleAwarenessUpdate = useCallback(() => {
    const awareness = yjsProvider?.awareness;
    setActiveUsers(
      Array.from(awareness ? awareness.getStates().entries() : []).map(
        ([userId, { color, name }]) => ({
          color,
          name,
          userId,
        })
      )
    );
  }, [yjsProvider]);

  useEffect(() => {
    if (yjsProvider == null) return;
    yjsProvider.awareness.on("update", handleAwarenessUpdate);

    return () => yjsProvider.awareness.off("update", handleAwarenessUpdate);
  }, [yjsProvider, handleAwarenessUpdate]);

  const createProvider = useCallback(
    (id: string, yjsDocMap: Map<string, Y.Doc>): Provider => {
      const doc = getDocFromMap(id, yjsDocMap);

      const provider = new WebsocketProvider("http://localhost:5154", id, doc, {
        connect: true,
      });

      // @ts-ignore
      setTimeout(() => setYjsProvider(provider), 0);

      // @ts-ignore
      return provider;
    },
    [providerName]
  );

  return (
    <LexicalComposer initialConfig={initialConfig}>
      <CollaborationPlugin
        id="ws"
        shouldBootstrap={false}
        providerFactory={createProvider}
        username={userProfile.name}
        cursorColor={userProfile.color}
      />
      <ToolbarPlugin myProfile={userProfile} otherUsers={activeUsers} />
      <ListPlugin />
      <div className={classes.editorContainer}>
        <RichTextPlugin
          contentEditable={
            <ContentEditable className={classes.editorContent} />
          }
          ErrorBoundary={LexicalErrorBoundary}
        />
      </div>
    </LexicalComposer>
  );
}
