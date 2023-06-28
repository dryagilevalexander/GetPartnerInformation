using System;
using System.Net;
using System.IO;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

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
string partnerInformation = "";
partnerInformation = objReader.ReadLine();

if (partnerInformation!= "{\"rows\":[]}") //Если организация найдена
{
    //Удаляем ненужные символы для приведения строки к json формату
    partnerInformation = partnerInformation.Replace("{\"rows\":", "");
    partnerInformation = partnerInformation.Replace("]}", "]");

    //Преобразуем строку в json
    List<JsonPartnerInformation> jsonPartnerInformation = JsonConvert.DeserializeObject<List<JsonPartnerInformation>>(partnerInformation);

    Console.WriteLine($"{jsonPartnerInformation[0].n} {jsonPartnerInformation[0].i} {jsonPartnerInformation[0].o}");
}
else Console.WriteLine("Организация не найдена");
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

public class JsonPartnerInformation
{
    public string a { get; set; }
    public string c { get; set; }
    public string g { get; set; }
    public string cnt { get; set; }
    public string i { get; set; }
    public string k { get; set; }
    public string o184 { get; set; }
    public string n { get; set; }
    public string o { get; set; }
    public string p { get; set; }
    public string r { get; set; }
    public string t { get; set; }
    public string pg { get; set; }
    public string tot { get; set; }






}