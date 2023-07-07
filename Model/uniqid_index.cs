namespace TodoApi;

// uniqid取得API返り値型定義
public class xs_query_get_uniqid_from_index_return
{
    public string[]? uniqID { get; set; }

    public int numberOfRow { get; set; }

    public int errorCode { get; set; }

    public void error(int code)
    {
        errorCode = code;
    }

    public string? sessionID { get; set; }
}

// uniqid取得API引数型定義
public class xs_query_get_uniqid_from_index
{
    private string queryObjectID { get; set; }

    private string tableName { get; set; }

    private int from { get; set; }

    private int to { get; set; }

    // コンストラクタ
    // 引数に入れられる型を定義します
    public xs_query_get_uniqid_from_index(string queryObjectID, string tableName, int from, int to)
    {
        this.queryObjectID = queryObjectID;
        this.tableName = tableName;
        this.from = from;
        this.to = to;
    }

    // APIのPOST用にオブジェクトの変数をJSON形式にするメソッド
    // 文字列で処理していますが見やすくするためにJSON形式にシリアライズしたい
    // (必須ではないためここではそのまま)
    public string toJson()
    {
        return
            "{\"xs-query-get-uniqid-from-index\": {" +
                $"\"queryObjectID\": \"{this.queryObjectID}\"," +
                $"\"tableName\": \"{this.tableName}\"," +
                $"\"from\": {this.from}," +
                $"\"to\": {this.to}" +
            "}}";
    }
}