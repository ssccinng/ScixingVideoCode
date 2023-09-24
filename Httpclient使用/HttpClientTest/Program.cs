// See https://aka.ms/new-console-template for more information
using HttpClientTest;
using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly;

var serviceCollection = new ServiceCollection();
serviceCollection.AddHttpClient();
serviceCollection.AddHttpClient("retry")
    .AddPolicyHandler(GetRetryPolicy());
serviceCollection.AddHttpClient("testClient", (client) =>
{
    client.BaseAddress = new Uri("http://localhost:5000");
    client.DefaultRequestHeaders.Add("aa", "linliu");
});

serviceCollection.AddHttpClient<LinliuService>((client) =>
{
    client.BaseAddress = new Uri("http://localhost:5000");
    client.DefaultRequestHeaders.Add("aa", "lanjing");

}).AddHttpMessageHandler<YiXiHandler>();



serviceCollection.AddScoped<YiXiHandler>();

serviceCollection.AddSingleton<TestService>();


var service = serviceCollection.BuildServiceProvider();


var httpClientFactory = service.GetRequiredService<IHttpClientFactory>();

var client = httpClientFactory.CreateClient();

var str = await client.GetStringAsync("https://www.baidu.com");
//Console.WriteLine(str);

var testService =  service.GetRequiredService<TestService>();
Console.WriteLine( await testService.GetString());
Console.WriteLine( await testService.GetHeader());


var linliuService = service.GetRequiredService <LinliuService>();
Console.WriteLine(await linliuService.GetString());
Console.WriteLine(await linliuService.GetHeader());
Console.WriteLine(await linliuService.TestPost());


MyClient myClient = new MyClient();
await myClient.PostAsync("http://www.baidu.com", new StringContent("yeshuanghun"));


var retryClient = httpClientFactory.CreateClient("retry");

await retryClient.GetAsync("http://www.baidu.com/sadasd");


IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    // 存在抖动策略
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(3, retryAttempt =>
        {

            Console.WriteLine(retryAttempt);
            return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
        });
}


class MyClient : HttpClient
{
    public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine(await request.Content.ReadAsStringAsync());
        return await base.SendAsync(request, cancellationToken);
    }
}


class YiXiHandler: DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content != null) 
            Console.WriteLine(await request.Content.ReadAsStringAsync());

        return await base.SendAsync(request, cancellationToken);
    }
}
