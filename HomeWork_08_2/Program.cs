﻿using HomeWork_08_2;

string fileForAnaliz = "..\\..\\..\\Connect.log";

//Виклик одноразово
//GeneratorOfLogs.Create(fileForAnaliz, 10000);

//Логгер помилок
Logger loggerError = new("..\\..\\..\\LogError.log");
loggerError.ExtDisplayAction = Console.WriteLine;

//для перенаправлення виводу результату
Logger Dispay = new("..\\..\\..\\DisplayResult.log");
Dispay.ExtDisplayAction = Console.WriteLine;

//Налаштування аналізатору логів
LogAnalyzer logAnalizer = new LogAnalyzer();
logAnalizer.LoggerErrorAdd += loggerError.Add;
logAnalizer.LoggerErrorAdd += Console.WriteLine;
logAnalizer.ExtDisplayAction = Console.WriteLine; //виводимо результат у консоль
logAnalizer.ExtDisplayAction += Dispay.Add; //дублюємо у файл

//Завантажимо файл для аналізу
logAnalizer.Load(fileForAnaliz);

//Розділимо по IP
var dicSplitByIP = logAnalizer.SplitByField((LogRecord record) => record.IPAdress);

//Статистика по IP
foreach (var recDic in dicSplitByIP)
{
    LogAnalyzer logAnalizerIP = recDic.Value;
    logAnalizerIP.ShowStat(recDic.Key.ToString()??"");
}

//Статистика повна
logAnalizer.ShowStat("All");









