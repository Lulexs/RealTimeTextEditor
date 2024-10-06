using System.Net.WebSockets;
using YDotNet.Document;
using YDotNet.Document.Transactions;
using YDotNet.Protocol;
using YDotNet.Server.WebSockets;

namespace API.Controllers;

public class CollaborationController : BaseController {
    private static readonly Dictionary<string, WSSharedDoc> docs = [];

    [Route("/ws")]
    public async Task Get() {
        if (HttpContext.WebSockets.IsWebSocketRequest) {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var docName = (HttpContext.Request.Path.ToString() ?? "").TrimStart('/');
            await SetupWSConnection(webSocket, docName);
        }
        else {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task SetupWSConnection(WebSocket conn, string docName) {
        var doc = getYDoc(docName);
        doc.Conns.Add(conn);

        await RecieveMessageAsync(conn, doc);
    }

    private async Task RecieveMessageAsync(WebSocket conn, WSSharedDoc doc) {
        var encoder = new WebSocketEncoder(conn);
        var decoder = new WebSocketDecoder(conn);

        while (true) {
            try {
                var msg = await decoder.ReadNextMessageAsync(CancellationToken.None);

                if (msg is SyncStep1Message msg1) {
                    Transaction readTransaction = doc.ReadTransaction();
                    var stateDiff = readTransaction.StateDiffV1(msg1.StateVector);
                    readTransaction.Commit();

                    await encoder.WriteAsync(new SyncStep2Message(stateDiff), CancellationToken.None);
                }
                else if (msg is SyncUpdateMessage msg2) {
                    Transaction writeTransaction = doc.WriteTransaction();
                    Console.WriteLine();
                    writeTransaction.ApplyV1(msg2.Update);
                    writeTransaction.Commit();

                    foreach (var sock in doc.Conns) {
                        if (sock == conn) continue;
                        var foreignEncoder = new WebSocketEncoder(sock); // AWFUL
                        await foreignEncoder.WriteAsync(msg2, CancellationToken.None);
                    }
                }
                else if (msg is AwarenessMessage msg3) {
                    foreach (var sock in doc.Conns) {
                        if (sock == conn) continue;
                        var foreignEncoder = new WebSocketEncoder(sock);
                        await foreignEncoder.WriteAsync(msg3, CancellationToken.None);
                    }                    
                }
            }
            catch (WebSocketException) {
                CloseConnectionAsync(doc, conn);
                break;
            }
        }

    }

    private void CloseConnectionAsync(WSSharedDoc doc, WebSocket conn) {
        doc.Conns.Remove(conn);

        if (doc.Conns.Count == 0) {
            docs.Remove(doc.Name);
        }
    }

    private WSSharedDoc getYDoc(string docName) {
        if (docs.TryGetValue(docName, out WSSharedDoc? value)) {
            return value;
        }
        value = new WSSharedDoc(docName);
        docs.Add(docName, value);
        return value;
    }
}

public class WSSharedDoc : Doc {
    public string Name { get; set; }
    public HashSet<WebSocket> Conns = [];

    public WSSharedDoc(string name) : base(new YDotNet.Document.Options.DocOptions() { SkipGarbageCollection = true }) {
        Name = name;

    }

}