using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    // 定数定義
    private readonly string TENANTID = "55ea3fa0f7ac473c8362c34ab9d93c53";
    private readonly string OBJECTID = "9ed58293355cf93b6040c9b58e944a74";
    private readonly string TABLE = "TodoApi";
    // POSTを行うメソッド
    // 同期処理のため返り値はTaskを使います
    private async Task<string> postAPI(string json)
    {
        // HTTPクライアント
        HttpClient client = new HttpClient();
        // リクエスト
        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, $"https://dev6ism7queryengine-{TENANTID}.qcs.center/api/v1");
        // 認証ヘッダ
        req.Headers.Add("Authorization", "APIQES");
        // bodyの中身
        req.Content = new StringContent(json, Encoding.UTF8, "application/json");
        // bodyの種類
        req.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        // POST処理
        HttpResponseMessage response = await client.SendAsync(req);
        // 結果がエラーならエラーthrow
        response.EnsureSuccessStatusCode();
        // 結果返却
        return await response.Content.ReadAsStringAsync();
    }

    [HttpGet(Name = "GetWeatherForecast")]
    // SwaggerでGETリクエストが行われたときの処理
    public async Task<string> Get()
    {
        // 処理開始時の時間を記録します
        DateTime start = DateTime.Now;
        // 見やすくするためコンソールの文字に色を付けます
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Start: {start}");
        // アイテムの行数をカウント
        // メソッド引数に型定義していてコード補完できるので入力ミスを減らせるかなと
        // 引数の型や数が合わないとエラーになるはずです
        xs_query_get_number_of_row rowCount = new xs_query_get_number_of_row(OBJECTID, TABLE);
        // APIが認識できるJSON形式に変換してからPOSTします
        string result = await postAPI(rowCount.toJson());
        // APIの返り値を数値に変換
        // 返り値は文字型なのでプログラムで扱いやすくするようJSON形式にデシリアライズします
        int? uniqIdCount = JsonSerializer.Deserialize<xs_query_get_number_of_row_return>(result)?.numberOfRow;
        // もしユニークIDがないなら(空)
        // 追加するデータの個数を設定します
        if (uniqIdCount == 0) uniqIdCount = 5;
        // POSTするデータを用意します
        // 毎回HTTP接続すると遅いので最後にまとめてPOSTします
        string postData = "";
        // ユニークIDの数だけ繰り返し
        for (int i = 0; i < uniqIdCount; i++)
        {
            // 送信するデータの準備
            // Dateは今日からの日付
            // TemperatureCは-20から55までの乱数
            // Summaryは定義済み文字配列からランダムな文字列を取り出します
            WeatherForecast weather = new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = WeatherForecast.Summaries[Random.Shared.Next(WeatherForecast.Summaries.Length)]
            };
            // 送信するデータの準備
            // 配列は毎回newしないといけないみたいです
            // (JSではいらないので気になります)
            xs_query_update update = new xs_query_update
            (
                OBJECTID,
                TABLE,
                new string[] { "date", "summary", "tempC", "tempF" },
                new string[] { weather.Date.ToString(), weather.Summary, weather.TemperatureC.ToString(), weather.TemperatureF.ToString() },
                (i + 1).ToString().PadLeft(32, '0'),
                true,
                true
            );
            // POST用データに追加します
            postData += update.toJson();
        }
        // 最後にまとめてデータをPOSTします
        await postAPI(postData);
        // 処理終了時の時間を記録します
        DateTime end = DateTime.Now;
        // 見やすくするためコンソールの文字に色を付けます
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"End: {end}");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(
            $"Duration: {Math.Round((end - start).TotalMilliseconds)}ms " +
            $"({Math.Floor((end - start).TotalSeconds)}s)\n"
        );
        // 最後にSwaggerに完了表示をして終わり
        // エラー例外処理を入れていないのでサーバーダウン時などの処理は入れておいたほうがいいかも
        return $"{uniqIdCount} 件追加または更新しました。\nQUERY SERVER PLAYGROUNDをご確認ください。\nhttps://dev6playground.qcs.center/";
    }
}
