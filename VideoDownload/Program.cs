using NReco.VideoConverter;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


string[] contents = new string[] { "video/avi", "application/json" };
var client = new HttpClient();
var video = app.MapGroup("/video");


app.UseStaticFiles();
app.MapGet("/", () => "Hello World!");

video.MapGet("/{*link}", async (string link, HttpContext con) =>
{

	var req = con.Request.RouteValues;
	var newlink = req.Values.First()!.ToString();
	var uri = new Uri(newlink!.ToString());



	var stream = await client.GetStreamAsync(uri);
	string path = Environment.CurrentDirectory;
	//var file = File.Create(path + "\\TextFile.txt");

	var filestream = File.Create(path + "\\newfile.txt");

	

	

	//
	await stream.CopyToAsync(filestream);
	await stream.FlushAsync();
	await stream.DisposeAsync();

		
	

	//await stream.CopyToAsync(file);

});

video.MapGet("/convert", (HttpContext ctx) =>
{
	var path = "C:\\Users\\MASTER\\Desktop\\video.mp4";
	//Stream outStream = File.Create(Environment.CurrentDirectory+"\\video.mp4");

	NReco.VideoConverter.FFMpegConverter cv = new();
	ConvertSettings settings = new ConvertSettings();
	OutputSettings outputSettings = new OutputSettings();
	outputSettings.VideoCodec = "h264";
	settings.VideoCodec = outputSettings.VideoCodec;
	cv.ConvertMedia(Environment.CurrentDirectory + "\\newfile.txt", "avi", Environment.CurrentDirectory + "\\video.mp4", "mp4", settings);

	//cv.ConvertMedia(Environment.CurrentDirectory + "\\IGnew.txt", Environment.CurrentDirectory + "\\video.mp4", "mp4");

	cv.ConvertProgress += Cv_ConvertProgress;

	#region MyRegion

	Results.File(Environment.CurrentDirectory + "\\video.mp4");
	Results.Ok(Environment.CurrentDirectory + "\\video.mp4");

	ctx.Response.WriteAsync(settings.VideoCodec.First().ToString());
	//ctx.Response.WriteAsync("""

	//	<!DOCTYPE html>
	//<html>
	//<head>
	//    <meta charset="utf-8" />
	//    <title>
	//        Video File
	//    </title>
	//</head>
	//<body>
	//    <p>
	//        Playing video
	//    </p>
	//    <div class="row">
	//     <video src="/Videos/video.avi" width="500" height="500" autoplay="autoplay" aria-live="assertive">
	//     okay
	//     </video>
	//    </div>


	//</body>
	//</html>

	//""");
	#endregion


});

void Cv_ConvertProgress(object? sender, ConvertProgressEventArgs e)
{
	Console.WriteLine("Total duration:{0}", e.TotalDuration);
	Console.WriteLine("Processed:{0}", e.Processed);
	Console.WriteLine();
}





//https://media.tokyoinsider.com/dl/00000000030/164510/1/cfmDbKYKNvJNPLhrNb67tQ/1/19/196493/196493/Dragon%20Ball%20GT%20-%2002.avi
//https://www.instagram.com/reel/Cxu4Hn5rXCx/?igshid=MzRlODBiNWFlZA== 
//https://www.instagram.com/p/CxyQRwJJH2A/?igshid=MzRlODBiNWFlZA== 
//https://scontent-waw1-1.cdninstagram.com/v/t66.30100-16/10000000_107337972471796_1766355590820748716_n.mp4?_nc_ht=scontent-waw1-1.cdninstagram.com&_nc_cat=104&_nc_ohc=xjTzr_weJbQAX_awKOm&edm=AP_V10EBAAAA&ccb=7-5&oh=00_AfDxr7BGl-_Of6KZeCCC71-mBtPS0XpTDm0vpHwL2crenw&oe=651838CA&_nc_sid=2999b8&dl=1&dl=1  
//https://media.tokyoinsider.com/dl/00000000030/165238/1/kwLyRkiqbVoff4-LOKxMzg/1/19/197262/197262/Dragon%20Ball%20GT%20-%2003.avi

//https://www.appsloveworld.com/wp-content/uploads/2018/10/640.mp4

app.Run();
