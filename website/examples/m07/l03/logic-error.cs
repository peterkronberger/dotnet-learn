// 5 of 14 events failed
int failures = 5;
int events = 14;
int share = failures / events * 100;
Console.WriteLine($"Failure share: {share} %");
Console.WriteLine($"Failure share, rounded: {Math.Round(failures * 100.0 / events)} %");
