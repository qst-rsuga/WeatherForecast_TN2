namespace TodoApi;

// uniqid取得API返り値型定義
public class xs_query_get_number_of_row_return
{
    public int numberOfRow { get; set; }

    public int errorCode { get; set; }

    public void error(int code)
    {
        errorCode = code;
    }

    public string? sessionID { get; set; }
}

// uniqid取得API引数型定義
public class xs_query_get_number_of_row
{
    private string queryObjectID { get; set; }

    private string tableName { get; set; }

    // コンストラクタ
    // 引数に入れられる型を定義します
    public xs_query_get_number_of_row(string queryObjectID, string tableName)
    {
        this.queryObjectID = queryObjectID;
        this.tableName = tableName;
    }

    // APIのPOST用にオブジェクトの変数をJSON形式にするメソッド
    // 文字列で処理していますが見やすくするためにJSON形式にシリアライズしたい
    // (必須ではないためここではそのまま)
    public string toJson()
    {
        return
            "{\"xs-query-get-number-of-row\": {" +
                $"\"queryObjectID\": \"{this.queryObjectID}\"," +
                $"\"tableName\": \"{this.tableName}\"," +
            "}}";
    }
}