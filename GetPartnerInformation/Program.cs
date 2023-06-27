using System;
using System.Net;
using System.IO;
using System.Net.Http.Json;
using System.Text.Json;

HttpClient httpClient = new HttpClient();

    var serverAddress = "https://egrul.nalog.ru/index.html";
    using var response = await httpClient.GetAsync(serverAddress);

    //Получаем куки с ключом из заголовков
    var dateValues = response.Headers.GetValues("Set-Cookie");
    string key = (dateValues.FirstOrDefault()).Split(';')[0];



//Формируем тело запроса к серверу
ReqBody reqBody = new ReqBody
{
    vyp3CaptchaToken = "",
    page = "",
    query = "7621007989",
    region = "",
    PreventChromeAutocomplete = ""
};

//Создаем тело запроса
JsonContent content = JsonContent.Create(reqBody);
// передаем ключ в заголовок
content.Headers.Add("Cookie", key);

//Отправляем запрос
var resp = await httpClient.PostAsync("https://egrul.nalog.ru/", content);
response.EnsureSuccessStatusCode();

//Получаем частичную ссылку на искомую организацию
string responseBody = await resp.Content.ReadAsStringAsync();

//Получаем полную ссылку
string sURL;
sURL = "https://egrul.nalog.ru/search-result/" + ((responseBody.Split(':')[1]).Split(",")[0]).Replace("\"", "");

//Отправляем Get запрос к серверу
WebRequest wrGETURL;
wrGETURL = WebRequest.Create(sURL);

//Получаем ответ
Stream objStream;
objStream = wrGETURL.GetResponse().GetResponseStream();
StreamReader objReader = new StreamReader(objStream);

string sLine = "";
int i = 0;

while (sLine != null)
{
    i++;
    sLine = objReader.ReadLine();
    if (sLine != null)
        Console.WriteLine("{0}:{1}", i, sLine);
}
Console.ReadLine();

class ReqBody
{
    public string vyp3CaptchaToken { get; set; }
    public string page { get; set; }
    public string query { get; set; }
    public string nameEq { get; set; }
    public string region { get; set; }
    public string PreventChromeAutocomplete { get; set; } 
}
