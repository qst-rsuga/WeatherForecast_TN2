namespace TodoApi;

// データアップデートAPI返り値型定義
public class xs_query_update_return
{
    public string? result { get; set; }

    public int errorCode { get; set; }

    public void error(int code)
    {
        errorCode = code;
    }

    public string? sessionID { get; set; }
}

// データアップデートAPI引数型定義
public class xs_query_update
{
    private string queryObjectID { get; set; }

    private string tableName { get; set; }

    // カラム名とアイテムは可変配列のため、要素を増減できない配列ではなく
    // 長さ可変のList型を使います
    private List<string> columnName { get; set; } = new List<string>();

    private List<string> item { get; set; } = new List<string>();

    private string uniqID { get; set; }

    private string flagInsert { get; set; }

    private string flagMulti { get; set; }

    // コンストラクタ
    // 引数に入れられる型を定義します
    public xs_query_update(string queryObjectID, string tableName, string[] columnName, string[] item, string uniqID, Boolean flagInsert, Boolean flagMulti)
    {
        this.queryObjectID = queryObjectID;
        this.tableName = tableName;
        // APIのPOSTデータは配列要素をダブルクォーテーションで括らないといけないため
        // それぞれの要素の前後に追加しています
        foreach (string i in columnName)
        {
            this.columnName.Add($"\"{i}\"");
        }
        foreach (string i in item)
        {
            this.item.Add($"\"{i}\"");
        }
        this.uniqID = uniqID;
        this.flagInsert = flagInsert.ToString();
        this.flagMulti = flagMulti.ToString();
    }

    // APIのPOST用にオブジェクトの変数をJSON形式にするメソッド
    // 文字列で処理していますが見やすくするためにJSON形式にシリアライズしたい
    // (必須ではないためここではそのまま)
    // List型をそのままAPIに送信することはできないため結合しています
    public string toJson()
    {
        return
            "{\"xs-query-update\": {" +
                $"\"queryObjectID\": \"{this.queryObjectID}\"," +
                $"\"tableName\": \"{this.tableName}\"," +
                $"\"columnName\": [{String.Join(',', this.columnName.ToArray())}]," +
                $"\"item\": [{String.Join(',', this.item.ToArray())}]," +
                $"\"uniqID\": \"{this.uniqID}\"," +
                $"\"flagInsert\": \"{this.flagInsert}\"," +
                $"\"flagMulti\": \"{this.flagMulti}\"" +
            "}}";
    }
}