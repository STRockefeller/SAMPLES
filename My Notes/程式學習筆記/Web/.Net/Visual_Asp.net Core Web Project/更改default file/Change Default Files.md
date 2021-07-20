DefaultFilesOptions options = new DefaultFilesOptions();
options.DefaultFileNames.Add("mydefault.html");
app.UseDefaultFiles(options);
app.UseStaticFiles();