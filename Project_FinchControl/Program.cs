using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control
    // Description: Application to control the Finch Robot
    // Application Type: Console
    // Author: Devin Wing
    // Dated Created: 10/1/2019
    // Last Modified: 11/2/2019
    //
    // **************************************************

    class Program
    {
        public enum Command
        {
            NONE,
            MOVEFORWARD,
            MOVEBACKWARD,
            STOPMOTORS,
            WAIT,
            TURNRIGHT,
            TURNLEFT,
            LEDON,
            LEDOFF,
            PLAYSONG,
            TEMPERATURE,
            DONE
        }

        static void Main(string[] args)
        {

            DisplayWelcomeScreen();

            DisplayMainMenu();

            DisplayClosingScreen();
        }

        #region main menu
        /// <summary>
        /// Display menu
        /// </summary>
        static void DisplayMainMenu()
        {
            //Instantiate a Finch Object
            Finch finchRobot = new Finch();

            bool finchRobotConnected = false;
            bool quitApplication = false;
            string menuChoice;

            do
            {
                Console.WriteLine();
                DisplayScreenHeader("Main Menu");

                //get user's menu choice
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine();
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine();
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine();
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine();
                Console.WriteLine("\te) User Programming");
                Console.WriteLine();
                Console.WriteLine("\tf) Change Theme");
                Console.WriteLine();
                Console.WriteLine("\tg) Disconnect Finch Robot");
                Console.WriteLine();
                Console.WriteLine("\tq) Quit");
                Console.WriteLine();
                Console.WriteLine("\tEnter Choice");
                menuChoice = Console.ReadLine().ToLower();

                //proccess user's choice
                switch (menuChoice)
                {
                    case "a":
                        finchRobotConnected = DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        if (finchRobotConnected)
                        {
                            DisplayTalentShow(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("\tPlease connect the finch robot to get started.");
                            DisplayContinuePrompt();
                        }
                        break;

                    case "c":
                        if (finchRobotConnected)
                        {
                            DisplayDataRecorder(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("\tPlease connect the finch robot to get started.");
                            DisplayContinuePrompt();
                        }

                        break;

                    case "d":
                        if (finchRobotConnected)
                        {
                            DisplayAlarmSystem(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("\tPlease connect the finch robot to get started.");
                            DisplayContinuePrompt();
                        }

                        break;

                    case "e":
                        if (finchRobotConnected)
                        {
                            DisplayUserProgramming(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("\tPlease connect the finch robot to get started.");
                            DisplayContinuePrompt();
                        }

                        break;

                    case "f":
                        DisplayChangeTheme();
                        break;

                    case "g":
                        DisplayDiconnectFinchRobot(finchRobot);
                        finchRobotConnected = false;
                        break;

                    case "q":
                        if (finchRobotConnected)
                        {
                            Console.WriteLine();
                            Console.WriteLine("\tPlease diconnect the Finch Robot before quitting.");
                            DisplayContinuePrompt();
                        }
                        else
                        {
                            quitApplication = true;
                        }
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease indicate your choice with the letter next to the option you wish to choose.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }
        #endregion

        #region change theme
        /// <summary>
        /// change theme
        /// </summary>
        static void DisplayChangeTheme()
        {
            string dataPath = "data\\Theme.txt";
            string[] foregroundColorString;
            ConsoleColor foregroundColor;

            DisplayScreenHeader("Change Theme");

            DisplayWriteTheme(dataPath);

            foregroundColorString = DisplayReadAllTheme(dataPath);

            foreach (string color in foregroundColorString)
            {
                Enum.TryParse(color, out foregroundColor);

                Console.ForegroundColor = foregroundColor;
            }


            DisplayContinuePrompt();

            DisplayMainMenu();
        }

        /// <summary>
        /// read theme file
        /// </summary>
        static string[] DisplayReadAllTheme(string dataPath)
        {
            string[] foregroundColorString;

            foregroundColorString = File.ReadAllLines(dataPath);

            return foregroundColorString;
        }

        /// <summary>
        /// user writes to the theme file
        /// </summary>
        static void DisplayWriteTheme(string dataPath)
        {
            bool validResponse = false;
            string userInput;

            do
            {
                DisplayScreenHeader("Change Theme");
                Console.WriteLine();
                Console.WriteLine("\tTheme Options:");
                Console.WriteLine();
                Console.WriteLine("\tGreen");
                Console.WriteLine("\tRed");
                Console.WriteLine("\tBlue");
                Console.WriteLine("\tMagenta");
                Console.WriteLine("\tWhite");
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("\tEnter your theme choice from the list above exaclty as it appears.");
                Console.WriteLine();

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "Green":
                        validResponse = true;
                        break;
                    case "Red":
                        validResponse = true;
                        break;
                    case "Blue":
                        validResponse = true;
                        break;
                    case "Magenta":
                        validResponse = true;
                        break;
                    case "White":
                        validResponse = true;
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPLEASE ENTER A CHOICE FROM THE LIST ABOVE EXACTLY AS IT APPEARS.");
                        Console.WriteLine();
                        DisplayContinuePrompt();
                        Console.Clear();
                        break;
                }
            } while (!validResponse);

            File.WriteAllText(dataPath, userInput);


        }
        #endregion

        #region user programming
        /// <summary>
        /// User programming
        /// </summary>
        static void DisplayUserProgramming(Finch finchRobot)
        {
            string menuChoice;
            bool quitApplication = false;
            List<Command> commands = new List<Command>();

            (int motorSpeed, int ledBrightness, int waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;


            do
            {


                DisplayScreenHeader("User Programming Menu");

                //get user's menu choice
                Console.WriteLine("\ta) Set Command Parameters");
                Console.WriteLine();
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine();
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine();
                Console.WriteLine("\td) View Parameters");
                Console.WriteLine();
                Console.WriteLine("\te) Execute Commands");
                Console.WriteLine();
                Console.WriteLine("\tf) Write Commands To Data File");
                Console.WriteLine();
                Console.WriteLine("\tg) Read Commands From Data File");
                Console.WriteLine();
                Console.WriteLine("\tq) Quit");
                Console.WriteLine();
                Console.WriteLine("\tEnter Choice");
                menuChoice = Console.ReadLine().ToLower();

                //proccess user's choice
                switch (menuChoice)
                {
                    case "a":
                        commandParameters = DisplayGetCommandParameters();
                        break;

                    case "b":
                        DislpayGetFinchCommands(commands);
                        break;

                    case "c":
                        DisplayFinchCommands(commands);
                        break;

                    case "d":
                        DisplayParamters(commandParameters);
                        break;

                    case "e":
                        DisplayExecuteFinchCommands(finchRobot, commands, commandParameters);
                        break;

                    case "f":
                        DisplayWriteUserProgrammingData(commands);
                        break;

                    case "g":
                        commands = DisplayReadUserProgrammingData();
                        break;

                    case "q":
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease indicate your choice with the letter next to the option you wish to choose.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        /// <summary>
        /// read command data file
        /// </summary>
        static List<Command> DisplayReadUserProgrammingData()
        {
            string dataPath = "data\\data.txt";
            List<Command> commands = new List<Command>();
            string[] commandsString;

            DisplayScreenHeader("View Saved Commands");

            Console.WriteLine();
            Console.WriteLine("\tReady to load saved commands.");
            Console.WriteLine();
            DisplayContinuePrompt();

            commandsString = File.ReadAllLines(dataPath);

            Command command;
            foreach (string commandString in commandsString)
            {
                Enum.TryParse(commandString, out command);

                commands.Add(command);
            }

            Console.WriteLine();
            Console.WriteLine("\tLoad Successful!");
            Console.WriteLine();

            DisplayContinuePrompt();

            return commands;
        }

        /// <summary>
        /// write to file
        /// </summary>
        static void DisplayWriteUserProgrammingData(List<Command> commands)
        {
            string dataPath = "data\\data.txt";
            List<string> commandsString = new List<string>();

            DisplayScreenHeader("Write Commands To Data File");

            Console.WriteLine();
            Console.WriteLine("\tReady to save commands.");
            Console.WriteLine();
            DisplayContinuePrompt();

            foreach (Command command in commands)
            {
                commandsString.Add(command.ToString());
            }

            File.WriteAllLines(dataPath, commandsString.ToArray());

            Console.WriteLine();
            Console.WriteLine("\tSave Successful!");

            DisplayContinuePrompt();

        }

        /// <summary>
        /// execute commands
        /// </summary>
        static void DisplayExecuteFinchCommands(Finch finchRobot, List<Command> commands,
            (int motorSpeed, int ledBrightness, int waitSeconds) commandParameters)
        {
            int motorSpeed = commandParameters.motorSpeed;
            int waitMilliSeconds = commandParameters.waitSeconds * 1000;
            int led = commandParameters.ledBrightness;

            double temp;

            DisplayScreenHeader("Execute Finch Commands");
            Console.WriteLine();
            Console.WriteLine("\tThe Finch will now execute your commands.");
            Console.WriteLine();
            DisplayContinuePrompt();


            Console.ReadKey();

            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:
                        break;
                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(motorSpeed, motorSpeed);
                        break;
                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-motorSpeed, -motorSpeed);
                        break;
                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        break;
                    case Command.WAIT:
                        finchRobot.wait(waitMilliSeconds);
                        break;
                    case Command.TURNLEFT:
                        finchRobot.setMotors(0, motorSpeed);
                        break;
                    case Command.TURNRIGHT:
                        finchRobot.setMotors(motorSpeed, 0);
                        break;
                    case Command.LEDOFF:
                        finchRobot.setLED(0, 0, 0);
                        break;
                    case Command.LEDON:
                        finchRobot.setLED(led, led, led);
                        break;
                    case Command.PLAYSONG:
                        DisplayPlayIntro(finchRobot);
                        break;
                    case Command.TEMPERATURE:
                        temp = finchRobot.getTemperature();
                        Console.WriteLine();
                        Console.WriteLine($"\tCurrent Temp: {temp}");
                        break;
                    default:
                        break;
                }
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display commands 
        /// </summary>
        static void DisplayFinchCommands(List<Command> commands)
        {
            Console.WriteLine();
            DisplayScreenHeader("Finch Robot Commands");
            Console.WriteLine();

            foreach (Command command in commands)
            {

                Console.WriteLine($"\t{command}");
                Console.WriteLine();
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// get commands
        /// </summary>
        static void DislpayGetFinchCommands(List<Command> commands)
        {
            Command command = Command.NONE;
            string userResponse;

            Console.WriteLine();
            while (command != Command.DONE)
            {
                DisplayScreenHeader("Finch Robot Commands");
                DisplayValidCommands();
                Console.WriteLine();
                Console.WriteLine("\tEnter 'DONE' when you have finished entering commands.");

                //ask for input
                Console.WriteLine();
                Console.Write("\tEnter Command: ");
                userResponse = Console.ReadLine().ToUpper();
                Enum.TryParse(userResponse, out command);

                //validate input
                switch (userResponse)
                {
                    case "NONE":
                        if (command != Command.NONE)
                        {
                            commands.Add(command);
                            Console.Clear();
                        }
                        break;
                    case "MOVEFORWARD":
                        if (command != Command.NONE)
                        {
                            commands.Add(command);
                            Console.Clear();
                        }
                        break;
                    case "MOVEBACKWARD":
                        if (command != Command.NONE)
                        {
                            commands.Add(command);
                            Console.Clear();
                        }
                        break;
                    case "STOPMOTORS":
                        if (command != Command.NONE)
                        {
                            commands.Add(command);
                            Console.Clear();
                        }
                        break;
                    case "WAIT":
                        if (command != Command.NONE)
                        {
                            commands.Add(command);
                            Console.Clear();
                        }
                        break;
                    case "TURNRIGHT":
                        if (command != Command.NONE)
                        {
                            commands.Add(command);
                            Console.Clear();
                        }
                        break;
                    case "TURNLEFT":
                        if (command != Command.NONE)
                        {
                            commands.Add(command);
                            Console.Clear();
                        }
                        break;
                    case "LEDON":
                        if (command != Command.NONE)
                        {
                            commands.Add(command);
                            Console.Clear();
                        }
                        break;
                    case "LEDOFF":
                        if (command != Command.NONE)
                        {
                            commands.Add(command);
                            Console.Clear();
                        }
                        break;
                    case "PLAYSONG":
                        if (command != Command.NONE)
                        {
                            commands.Add(command);
                            Console.Clear();
                        }
                        break;
                    case "TEMPERATURE":
                        if (command != Command.NONE)
                        {
                            commands.Add(command);
                            Console.Clear();
                        }
                        break;
                    case "DONE":
                        if (command != Command.NONE)
                        {
                            commands.Add(command);
                            Console.Clear();
                        }
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPLEASE ENTER A CHOICE FROM THE LIST ABOVE.");
                        Console.WriteLine();
                        DisplayContinuePrompt();
                        Console.Clear();
                        break;
                }


            }

            //echo user inputs
            Console.WriteLine();
            Console.WriteLine("\tCommands Entered: ");
            Console.WriteLine();

            foreach (Command input in commands)
            {
                Console.WriteLine($"\t{input}");
                Console.WriteLine();
            }

            Console.WriteLine("\tEntered commands can also be viewed from the 'View Commands' option in the menu.");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// get parameters
        /// </summary>
        static (int motorSpeed, int ledBrightness, int waitSeconds) DisplayGetCommandParameters()
        {
            (int motorSpeed, int ledBrightness, int waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            bool validResponse1 = false;
            bool validResponse2 = false;
            bool validResponse3 = false;

            //motor speed
            do
            {
                Console.Clear();
                DisplayScreenHeader("Get Parameters");
                Console.WriteLine();
                Console.Write("\tEnter Motor Speed [1 - 255]: ");
                if (int.TryParse(Console.ReadLine(), out commandParameters.motorSpeed) && (commandParameters.motorSpeed <= 255) && (commandParameters.motorSpeed >= 1))
                {
                    validResponse1 = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter an integer between 1 and 255.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                    Console.Clear();
                }
            } while (!validResponse1);

            do
            {
                //led brightness
                Console.Clear();
                DisplayScreenHeader("Get Parameters");
                Console.WriteLine();
                Console.Write("\tEnter LED Brightness [1 - 255]: ");
                if (int.TryParse(Console.ReadLine(), out commandParameters.ledBrightness) && (commandParameters.ledBrightness <= 255) && (commandParameters.ledBrightness >= 1))
                {
                    validResponse2 = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter an integer between 1 and 255.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                    Console.Clear();
                }
            } while (!validResponse2);

            do
            {
                //wait duration
                Console.Clear();
                DisplayScreenHeader("Get Parameters");
                Console.WriteLine();
                Console.Write("\tEnter Wait Duration [seconds]: ");
                if (int.TryParse(Console.ReadLine(), out commandParameters.waitSeconds) && (commandParameters.waitSeconds >= 1))
                {
                    validResponse3 = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter an integer greater than 1.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                    Console.Clear();
                }
            } while (!validResponse3);

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\tThe Parameters Will Be Set To The Following:");
            Console.WriteLine();
            Console.WriteLine($"\tMotor Speed: {commandParameters.motorSpeed}");
            Console.WriteLine();
            Console.WriteLine($"\tLED Brightness: {commandParameters.ledBrightness}");
            Console.WriteLine();
            Console.WriteLine($"\tWait Duration: {commandParameters.waitSeconds} seconds");
            Console.WriteLine();
            Console.WriteLine("\tThese parameters can also be viewed by selecting 'View Parameters' in the menu.");

            DisplayContinuePrompt();

            return commandParameters;
        }

        /// <summary>
        /// display the user set parameters
        /// </summary>
        static void DisplayParamters((int motorSpeed, int ledBrightness, int waitSeconds) commandParameters)
        {
            Console.WriteLine();
            DisplayScreenHeader("User Parameters");

            Console.WriteLine();
            Console.WriteLine("\tThe Parameters are set to the following: ");
            Console.WriteLine();
            Console.WriteLine($"\tMotor Speed: {commandParameters.motorSpeed}");
            Console.WriteLine();
            Console.WriteLine($"\tLED Brightness: {commandParameters.ledBrightness}");
            Console.WriteLine();
            Console.WriteLine($"\tWait Duration: {commandParameters.waitSeconds} seconds");

            DisplayContinuePrompt();
        }

        #endregion

        #region alarm system
        /// <summary>
        /// Alarm System
        /// </summary>
        static void DisplayAlarmSystem(Finch finchRobot)
        {

            DisplayScreenHeader("The Finch Alarm System");

            string alarmType = DisplayGetAlarmType();

            int maxSeconds = DisplayGetMaxSeconds();

            if (alarmType == "light")
            {
                double thresholdLightMin = DisplayGetThresholdLightMin(finchRobot, alarmType);
                double thresholdLightMax = DisplayGetThresholdLightMax(finchRobot, alarmType);

                bool lightThresholdExceeded = MonitorCurrentLightLevels(finchRobot, thresholdLightMin, thresholdLightMax, maxSeconds);

                if (lightThresholdExceeded)
                {
                    finchRobot.setLED(255, 0, 0);
                    finchRobot.noteOn(600);
                    finchRobot.wait(300);
                    finchRobot.noteOff();
                    finchRobot.noteOn(600);
                    finchRobot.wait(300);
                    finchRobot.noteOff();
                    finchRobot.setLED(0, 0, 0);

                    Console.WriteLine();
                    Console.WriteLine("\tYou have exceeded your threshold.");
                }
                else
                {
                    finchRobot.setLED(0, 0, 255);
                    finchRobot.noteOn(600);
                    finchRobot.wait(300);
                    finchRobot.noteOff();
                    finchRobot.setLED(0, 0, 0);

                    Console.WriteLine();
                    Console.WriteLine("\tYou have exceeded your maximum time.");
                }
            }
            else if (alarmType == "temperature")
            {
                double thresholdTempmin = DisplayGetThresholdTempMin(finchRobot, alarmType);
                double thresholdTempMax = DisplayGetThresholdTempMax(finchRobot, alarmType);

                bool tempThresholdExceeded = MonitorCurrentTemperature(finchRobot, thresholdTempmin, thresholdTempMax, maxSeconds);

                if (tempThresholdExceeded)
                {
                    finchRobot.setLED(255, 0, 0);
                    finchRobot.noteOn(600);
                    finchRobot.wait(300);
                    finchRobot.noteOff();
                    finchRobot.noteOn(600);
                    finchRobot.wait(300);
                    finchRobot.noteOff();
                    finchRobot.setLED(0, 0, 0);

                    Console.WriteLine();
                    Console.WriteLine("\tYou have exceeded your threshold.");
                }
                else
                {
                    finchRobot.setLED(0, 0, 255);
                    finchRobot.noteOn(600);
                    finchRobot.wait(300);
                    finchRobot.noteOff();
                    finchRobot.setLED(0, 0, 0);

                    Console.WriteLine();
                    Console.WriteLine("\tYou have exceeded you maximum time.");
                }
            }


            DisplayReturnMainMenuPrompt();
        }

        /// <summary>
        /// monitor temperature
        /// </summary>
        static bool MonitorCurrentTemperature(Finch finchRobot, double thresholdTempMin, double thresholdTempMax, int maxSeconds)
        {
            bool thresholdExceeded = false;
            double counter = 0;

            do
            {
                double temp = finchRobot.getTemperature();
                DisplayScreenHeader("Temperatures");

                Console.WriteLine($"\tMinimum Temperature: {thresholdTempMin}");
                Console.WriteLine();
                Console.WriteLine($"\tMaximum Temperature: {thresholdTempMax}");
                Console.WriteLine();
                Console.WriteLine($"\tCurrent Temperature: {temp}");

                finchRobot.wait(500);
                counter += 0.5;

                if ((thresholdTempMax < temp || temp < thresholdTempMin))
                {
                    thresholdExceeded = true;
                }

            } while (!thresholdExceeded && counter <= maxSeconds);


            return thresholdExceeded;
        }

        /// <summary>
        /// monitor light levels
        /// </summary>
        static bool MonitorCurrentLightLevels(Finch finchRobot, double thresholdLightMin, double thresholdLightMax, int maxSeconds)
        {
            bool thresholdExceeded = false;
            double counter = 0;

            do
            {
                int currentLightLevel = finchRobot.getLeftLightSensor();
                DisplayScreenHeader("Light Levels");

                Console.WriteLine($"\tMinimum Light Level: {thresholdLightMin}");
                Console.WriteLine();
                Console.WriteLine($"\tMaximum Light Level: {thresholdLightMax}");
                Console.WriteLine();
                Console.WriteLine($"\tCurrent Light Level: {currentLightLevel}");

                finchRobot.wait(500);
                counter += .5;

                if ((thresholdLightMax < currentLightLevel) || (currentLightLevel < thresholdLightMin))
                {
                    thresholdExceeded = true;
                }

            } while (!thresholdExceeded && (counter <= maxSeconds));

            return thresholdExceeded;
        }

        /// <summary>
        /// temperature theshold max
        /// </summary>
        static double DisplayGetThresholdTempMax(Finch finchRobot, string alarmType)
        {
            double threshold = 0;
            string input;
            bool validResponse = false;

            do
            {
                switch (alarmType)
                {
                    case "temperature":
                        Console.Clear();
                        DisplayScreenHeader("Threshold Value");
                        Console.WriteLine();
                        Console.Write($"\tCurrent Temperature: {finchRobot.getTemperature()}");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("\tEnter MAXIMUM Temperature(In Celsius): ");
                        input = Console.ReadLine();
                        if (double.TryParse(input, out threshold))
                        {
                            validResponse = true;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("\tPlease enter a valid temperature.");
                            DisplayContinuePrompt();
                        }
                        break;

                    default:
                        Console.WriteLine("\tNo valid temperature entered.");
                        break;
                }
            } while (!validResponse);

            return threshold;
        }

        /// <summary>
        /// temperature threshold min
        /// </summary>
        static double DisplayGetThresholdTempMin(Finch finchRobot, string alarmType)
        {
            double threshold = 0;
            string input;
            bool validResponse = false;

            do
            {
                switch (alarmType)
                {
                    case "temperature":
                        Console.Clear();
                        DisplayScreenHeader("Threshold Value");
                        Console.WriteLine();
                        Console.Write($"\tCurrent Temperature: {finchRobot.getTemperature()}");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("\tEnter MINIMUM Temperature(In Celsius): ");
                        input = Console.ReadLine();
                        if (double.TryParse(input, out threshold))
                        {
                            validResponse = true;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("\tPlease enter a valid temperature.");
                            DisplayContinuePrompt();
                        }
                        break;

                    default:
                        Console.WriteLine("\tNo valid temperature entered.");
                        break;
                }
            } while (!validResponse);

            return threshold;
        }

        /// <summary>
        /// light threshold max
        /// </summary>
        static double DisplayGetThresholdLightMax(Finch finchRobot, string alarmType)
        {
            string input;
            double threshold = 0;
            bool validResponse = false;

            do
            {
                switch (alarmType)
                {

                    case "light":
                        Console.Clear();
                        DisplayScreenHeader("Threshold Value");
                        Console.WriteLine();
                        Console.Write($"\tCurrent Light Level: {finchRobot.getRightLightSensor()}");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("\tEnter MAXIMUM Light Level (0-255): ");
                        input = Console.ReadLine();
                        if (double.TryParse(input, out threshold) && threshold > 0)
                        {
                            validResponse = true;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("\tPlease enter a value from 0 to 255");
                            DisplayContinuePrompt();
                        }
                        break;
                    default:
                        Console.WriteLine("\tNo valid light level entered.");
                        break;
                }
            } while (!validResponse);


            DisplayContinuePrompt();

            return threshold;
        }

        /// <summary>
        /// light threshold min
        /// </summary>
        static double DisplayGetThresholdLightMin(Finch finchRobot, string alarmType)
        {
            string input;
            double threshold = 0;
            bool validResponse = false;

            do
            {
                switch (alarmType)
                {

                    case "light":
                        Console.Clear();
                        DisplayScreenHeader("Threshold Value");
                        Console.WriteLine();
                        Console.Write($"\tCurrent Light Level: {finchRobot.getRightLightSensor()}");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("\tEnter MINIMUM Light Level (0-255): ");
                        input = Console.ReadLine();
                        if (double.TryParse(input, out threshold) && threshold > 0)
                        {
                            validResponse = true;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("\tPlease enter a value from 0 to 255");
                            DisplayContinuePrompt();
                        }
                        break;
                    default:
                        Console.WriteLine("\tNo valid light level entered.");
                        break;
                }
            } while (!validResponse);


            DisplayContinuePrompt();

            return threshold;
        }

        /// <summary>
        /// get seconds for alarm
        /// </summary>
        static int DisplayGetMaxSeconds()
        {
            string input;
            int userResponse;
            bool validResponse = false;

            do
            {
                Console.Clear();
                DisplayScreenHeader("Get Duration");
                Console.WriteLine();
                Console.Write("\tEnter maximum number of seconds: ");
                input = Console.ReadLine();

                if (int.TryParse(input, out userResponse) && userResponse > 0)
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a number greater than 0.");
                }
                DisplayContinuePrompt();
            } while (!validResponse);


            return userResponse;
        }

        /// <summary>
        /// get alarm type
        /// </summary>
        static string DisplayGetAlarmType()
        {
            bool validResponse = false;
            string userResponse;

            do
            {
                Console.Clear();
                DisplayScreenHeader("Get Alarm Type");
                Console.WriteLine();
                Console.Write("\tEnter alarm type (light or temperature): ");
                userResponse = Console.ReadLine().ToLower();

                switch (userResponse)
                {
                    case "light":
                        validResponse = true;
                        break;
                    case "temperature":
                        validResponse = true;
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter either light or temperature.");
                        break;
                }
                DisplayContinuePrompt();
            } while (!validResponse);



            return userResponse;
        }

        #endregion

        #region data recorder
        /// <summary>
        /// Data recorder
        /// </summary>
        static void DisplayDataRecorder(Finch finchRobot)
        {
            double dataPointFrequency;
            double dataPointFrequencyLight;
            int numberOfDataPointsLight;
            int numberOfDataPoints;
            bool validResponse = false;
            string userChoice;



            do
            {
                Console.Clear();
                DisplayScreenHeader("Finch Robot Data Recorder");
                Console.WriteLine();
                Console.WriteLine("\tWelcome to the data recorder.");
                Console.WriteLine();
                Console.WriteLine("\tChoose the type of data you wish me to get.");
                Console.WriteLine("\tThen enter the duration (the frequency) and");
                Console.WriteLine("\tthe number of times (data points) you");
                Console.WriteLine("\twish me to collect data.");
                Console.WriteLine();
                Console.WriteLine("\tEnter temperature to get temperature data.");
                Console.WriteLine();
                Console.WriteLine("\tEnter light to get light sensor information.");
                Console.WriteLine();
                userChoice = Console.ReadLine().ToLower();

                switch (userChoice)
                {
                    case "temperature":
                    case "light":
                        validResponse = true;
                        break;
                    default:
                        Console.WriteLine("\tPlease enter either temp or light");
                        break;

                }

                Console.WriteLine();
                DisplayContinuePrompt();

            } while (!validResponse);



            if (validResponse)
            {
                if (userChoice == "temperature")
                {
                    dataPointFrequency = DisplayGetDataPointFrequency();
                    numberOfDataPoints = DisplayGetNumberOfDataPoints();
                    double[] temperatures = new double[numberOfDataPoints];
                    DisplayGetData(numberOfDataPoints, dataPointFrequency, temperatures, finchRobot);
                    DisplayData(temperatures);
                }
                else if (userChoice == "light")
                {
                    dataPointFrequencyLight = DisplayGetFrequencyForLight();
                    numberOfDataPointsLight = DisplayGetNumberOfPointsLight();
                    double[] light = new double[numberOfDataPointsLight];
                    DisplayGetLightData(numberOfDataPointsLight, dataPointFrequencyLight, light, finchRobot);
                    DisplayLightData(light);
                }
            }





            DisplayReturnMainMenuPrompt();
        }

        /// <summary>
        /// display light data
        /// </summary>
        private static void DisplayLightData(double[] light)
        {
            DisplayScreenHeader("Light Recordings");

            Console.WriteLine();
            Console.WriteLine("\tLight recordings are displayed in a range 0 (darkest) to 255 (lightest)");
            Console.WriteLine();

            for (int i = 0; i < light.Length; i++)
            {
                Console.WriteLine($"\tLight Recordings: {i + 1}: {light[i]}");
                Console.WriteLine();
            }

        }

        /// <summary>
        /// get light recordings
        /// </summary>
        private static void DisplayGetLightData(double dataPointFrequencyLight, double numberOfDataPointsLight, double[] light, Finch finchRobot)
        {
            DisplayScreenHeader("Get Light Recordings");
            Console.WriteLine();
            Console.WriteLine("\tI will now gather LIGHT DATA. ");
            Console.WriteLine();
            DisplayContinuePrompt();

            for (int i = 0; i < numberOfDataPointsLight; i++)
            {
                light[i] = finchRobot.getRightLightSensor();
                int milliseconds = (int)(dataPointFrequencyLight * 1000);
                finchRobot.wait(milliseconds);

                Console.WriteLine();
                Console.WriteLine($"\tLight Recordings: {i + 1}: {light[i]}");
            }

            Console.WriteLine();
            DisplayContinuePrompt();
        }

        /// <summary>
        /// get number of data points for light
        /// </summary>
        private static int DisplayGetNumberOfPointsLight()
        {
            int numberOfDataPointsLight;
            bool validResponse = false;
            string userResponse;

            do
            {
                Console.Clear();
                DisplayScreenHeader("Number of Data Points");

                Console.WriteLine();
                Console.Write("\tEnter the number of data points: ");
                Console.WriteLine();
                userResponse = Console.ReadLine();

                if (int.TryParse(userResponse, out numberOfDataPointsLight) && numberOfDataPointsLight > 0)
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tENTER THE NUMBER OF DATAPOINTS AS AN INTEGER GREATER THAN ZERO.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                }

            } while (!validResponse);


            DisplayContinuePrompt();

            return numberOfDataPointsLight;
        }

        /// <summary>
        /// get light frequency
        /// </summary>
        static double DisplayGetFrequencyForLight()
        {
            string userResponse;
            bool validResponse = false;
            double dataPointFrequencyLight;



            do
            {
                Console.Clear();
                DisplayScreenHeader("Data Point Frequency");

                Console.WriteLine();
                Console.Write("\tEnter frequency of light recordings in seconds: ");
                Console.WriteLine();
                userResponse = Console.ReadLine();

                if (double.TryParse(userResponse, out dataPointFrequencyLight) && dataPointFrequencyLight > 0)
                {
                    validResponse = true;
                }

                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tENTER FREQUENCY AS AN INTEGER GREATER THAN ZERO.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                }

            } while (!validResponse);

            DisplayContinuePrompt();

            return dataPointFrequencyLight;
        }

        /// <summary>
        /// display the temperature data
        /// </summary>
        static void DisplayData(double[] temperatures)
        {
            DisplayScreenHeader("Temperatures");
            Console.WriteLine();

            for (int i = 0; i < temperatures.Length; i++)
            {
                Console.WriteLine($"\tTemperatures: {i + 1}: {temperatures[i]} degrees Celsius");
                Console.WriteLine();
            }

        }

        /// <summary>
        /// get data with the finch
        /// </summary>
        static void DisplayGetData(int numberOfDataPoints, double dataPointFrequency, double[] temperatures, Finch finchRobot)
        {
            int tempF;

            DisplayScreenHeader("Get Temperatures");
            Console.WriteLine();
            Console.WriteLine("\tI will now gather TEMPERATURE DATA. ");
            Console.WriteLine();
            DisplayContinuePrompt();

            for (int i = 0; i < numberOfDataPoints; i++)
            {
                temperatures[i] = finchRobot.getTemperature();
                int milliseconds = (int)(dataPointFrequency * 1000);
                finchRobot.wait(milliseconds);

                tempF = (int)(temperatures[i] * 1.8 + 32);
                Console.WriteLine();
                Console.WriteLine($"\tTemperature (Celsius): {i + 1}: {temperatures[i]}");
                Console.WriteLine();
                Console.WriteLine($"\tTemperatures (Fahrenheit): {i + 1}: {tempF}");
            }

            Console.WriteLine();
            DisplayContinuePrompt();

        }

        /// <summary>
        /// get data points
        /// </summary>
        static int DisplayGetNumberOfDataPoints()
        {
            bool validResponse = false;
            int numberOfDataPoints;
            string userResponse;

            do
            {
                Console.Clear();
                DisplayScreenHeader("Number of Data Points");

                Console.WriteLine();
                Console.Write("\tEnter the number of data points: ");
                Console.WriteLine();
                userResponse = Console.ReadLine();

                if (int.TryParse(userResponse, out numberOfDataPoints) && numberOfDataPoints > 0)
                {
                    validResponse = true;
                }

                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tENTER NUMBER OF DATA POINTS AS AN INTEGER GREATER THAN ZERO");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                }

            } while (!validResponse);

            Console.WriteLine();
            DisplayContinuePrompt();

            return numberOfDataPoints;
        }

        /// <summary>
        /// get frequency
        /// </summary>
        static double DisplayGetDataPointFrequency()
        {
            bool validResponse = false;
            double dataPointFrequency;
            string userResponse;

            do
            {
                Console.Clear();
                DisplayScreenHeader("Data Point Frequency");
                Console.WriteLine();
                Console.Write("\tEnter frequency of temperature recordings in seconds: ");
                Console.WriteLine();
                userResponse = Console.ReadLine();

                if (double.TryParse(userResponse, out dataPointFrequency) && dataPointFrequency > 0)
                {
                    validResponse = true;
                }

                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tENTER FREQUENCY AS AN INTEGER GREATER THAN ZERO");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                }
            } while (!validResponse);

            DisplayContinuePrompt();

            return dataPointFrequency;
        }
        #endregion

        #region Talent Show
        /// <summary>
        /// Talent show
        /// </summary>
        static void DisplayTalentShow(Finch finchRobot)
        {
            string userResponse;

            DisplayScreenHeader("Finch Robot Talent Show");

            Console.WriteLine("\tThe Finch Robot is ready to show you what it can do!");
            Console.WriteLine();

            //talent show introduction
            DisplayPlayIntro(finchRobot);

            //light show
            DisplayLightShow(finchRobot);

            //finch moves
            DisplayFinchRobotMove(finchRobot);

            //user selects a desired option
            Console.WriteLine();
            userResponse = DisplayUserMovementChoice();

            //perform user's choice
            DisplayDoUserChoice(finchRobot, userResponse);

            //plays exit song
            DisplayPlayExit(finchRobot);

            //return to main menu
            DisplayReturnMainMenuPrompt();
        }

        /// <summary>
        /// Finch robot moves for talent show
        /// </summary>
        static void DisplayFinchRobotMove(Finch finchRobot)
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to watch me whip");
            Console.ReadKey();

            finchRobot.wait(300);
            finchRobot.noteOn(1000);
            finchRobot.wait(500);
            finchRobot.noteOn(1500);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(500);

            //light on
            finchRobot.setLED(0, 255, 0);

            //forward
            for (int i = 0; i < 255; i = i + 10)
            {
                finchRobot.setMotors(i, i);
            }
            finchRobot.wait(2000);
            finchRobot.setMotors(0, 0);

            //reverse
            finchRobot.setLED(255, 0, 0);
            for (int i = 0; i > -255; i = i - 10)
            {
                finchRobot.setMotors(i, i);
            }
            finchRobot.wait(2000);
            finchRobot.setMotors(0, 0);

            finchRobot.wait(1000);
            //spin

            finchRobot.setLED(0, 0, 255);
            finchRobot.setMotors(255, -255);
            finchRobot.wait(1500);
            //walk forward

            finchRobot.setLED(0, 255, 0);
            finchRobot.setMotors(100, 0);
            finchRobot.wait(500);
            finchRobot.setMotors(0, 100);
            finchRobot.wait(500);
            finchRobot.setMotors(100, 0);
            finchRobot.wait(500);
            finchRobot.setMotors(0, 100);
            finchRobot.wait(500);
            finchRobot.setMotors(100, 0);
            finchRobot.wait(500);
            finchRobot.setMotors(0, 100);
            finchRobot.wait(500);
            finchRobot.setMotors(100, 0);
            finchRobot.wait(500);
            finchRobot.setMotors(0, 100);
            finchRobot.wait(500);
            finchRobot.setMotors(100, 100);
            finchRobot.wait(500);
            finchRobot.setMotors(0, 0);
            finchRobot.wait(500);

            //walk back
            finchRobot.setLED(255, 0, 0);
            finchRobot.setMotors(-100, 0);
            finchRobot.wait(500);
            finchRobot.setMotors(0, -100);
            finchRobot.wait(500);
            finchRobot.setMotors(-100, 0);
            finchRobot.wait(500);
            finchRobot.setMotors(0, -100);
            finchRobot.wait(500);
            finchRobot.setMotors(-100, 0);
            finchRobot.wait(500);
            finchRobot.setMotors(0, -100);
            finchRobot.wait(500);
            finchRobot.setMotors(-100, 0);
            finchRobot.wait(500);
            finchRobot.setMotors(0, -100);
            finchRobot.wait(500);
            finchRobot.setMotors(-100, -100);
            finchRobot.wait(500);
            finchRobot.setMotors(0, 0);
            finchRobot.wait(500);

            //spin
            finchRobot.setLED(0, 0, 255);
            finchRobot.setMotors(255, -255);
            finchRobot.wait(1500);
            //stop
            finchRobot.setMotors(0, 0);
            //light off
            finchRobot.setLED(0, 0, 0);
        }

        /// <summary>
        /// finch performs user's choice
        /// </summary>
        static void DisplayDoUserChoice(Finch finchRobot, string userResponse)
        {
            finchRobot.wait(300);
            finchRobot.noteOn(1000);
            finchRobot.wait(500);
            finchRobot.noteOn(1500);
            finchRobot.wait(500);
            finchRobot.noteOff();
            finchRobot.wait(1000);

            if (userResponse == "spin")
            {
                finchRobot.setLED(0, 0, 255);
                finchRobot.setMotors(255, -255);
                finchRobot.wait(2000);
                finchRobot.setMotors(0, 0);
                finchRobot.setLED(0, 0, 0);
            }
            else if (userResponse == "speed")
            {
                finchRobot.setLED(0, 255, 0);
                finchRobot.setMotors(255, 255);
                finchRobot.wait(2000);
                finchRobot.setMotors(0, 0);
                finchRobot.setLED(0, 0, 0);
            }

            //DisplayContinuePrompt();
        }

        /// <summary>
        /// user makes a choice
        /// </summary>
        static string DisplayUserMovementChoice()
        {
            string userResponse;
            bool validResponse = false;

            do
            {
                Console.WriteLine();
                Console.WriteLine("\tWould you like me to spin or would you like me to show you how fast I am?");
                Console.WriteLine();
                Console.WriteLine("\tEnter spin or speed: ");
                Console.WriteLine();
                userResponse = Console.ReadLine();

                switch (userResponse)
                {
                    case "spin":
                    case "speed":
                        validResponse = true;
                        break;
                    default:
                        Console.WriteLine("\tPlease enter either spin or speed.");
                        break;
                }
            } while (!validResponse);

            return userResponse;
        }

        /// <summary>
        /// Talent Show Light Show
        /// </summary>
        static void DisplayLightShow(Finch finchRobot)
        {

            Console.WriteLine("\tPress any key and I will dazzle you with some lights.");
            Console.ReadKey();

            finchRobot.wait(300);
            finchRobot.noteOn(1000);
            finchRobot.wait(500);
            finchRobot.noteOn(1500);
            finchRobot.wait(500);
            finchRobot.noteOff();


            //red
            finchRobot.wait(1000);
            finchRobot.setLED(255, 0, 0);
            finchRobot.wait(500);
            //green
            finchRobot.setLED(0, 255, 0);
            finchRobot.wait(500);
            //blue
            finchRobot.setLED(0, 0, 255);
            finchRobot.wait(500);
            //red
            finchRobot.setLED(255, 0, 0);
            finchRobot.wait(500);
            //red down
            for (int i = 255; i > 0; i = i - 5)
            {
                finchRobot.setLED(i, 0, 0);
            }
            finchRobot.wait(300);
            //ramp up green
            for (int i = 0; i < 255; i = i + 5)
            {
                finchRobot.setLED(0, i, 0);
            }
            //green down
            for (int i = 255; i > 0; i = i - 5)
            {
                finchRobot.setLED(0, i, 0);
            }
            finchRobot.wait(300);
            //ramp up blue
            for (int i = 0; i < 255; i = i + 5)
            {
                finchRobot.setLED(0, 0, i);
            }
            //blue down
            for (int i = 255; i > 0; i = i - 5)
            {
                finchRobot.setLED(0, 0, i);
            }
            finchRobot.wait(500);

            finchRobot.setLED(0, 0, 0);
        }

        /// <summary>
        /// Play Talent Show Intro Song
        /// </summary>
        static void DisplayPlayIntro(Finch finchRobot)
        {
            finchRobot.setLED(255, 255, 255);
            finchRobot.noteOn(659);
            finchRobot.wait(200);
            //
            finchRobot.noteOn(659);
            finchRobot.wait(300);
            //
            finchRobot.noteOn(659);
            finchRobot.wait(300);
            //
            finchRobot.noteOn(523);
            finchRobot.wait(225);
            //
            finchRobot.noteOn(659);
            finchRobot.wait(225);
            //
            finchRobot.noteOn(784);
            finchRobot.wait(225);
            //
            finchRobot.noteOn(392);
            finchRobot.wait(300);
            //
            finchRobot.noteOff();
            finchRobot.setLED(0, 0, 0);
        }

        /// <summary>
        /// plays exit song
        /// </summary>
        static void DisplayPlayExit(Finch finchRobot)
        {
            Console.WriteLine();
            Console.WriteLine("\tThanks for watching the talent show.");

            finchRobot.setLED(255, 0, 0);
            finchRobot.wait(2500);
            finchRobot.noteOn(440);
            finchRobot.wait(300);
            //
            finchRobot.noteOn(523);
            finchRobot.wait(300);
            //
            finchRobot.noteOn(587);
            finchRobot.wait(300);
            //
            finchRobot.noteOn(622);
            finchRobot.wait(225);
            //
            finchRobot.noteOn(587);
            finchRobot.wait(225);
            //
            finchRobot.noteOn(523);
            finchRobot.wait(225);
            //
            finchRobot.noteOff();
            finchRobot.setLED(0, 0, 0);
        }
        #endregion

        #region Connect/ Disconnect
        /// <summary>
        /// disconnect the finch robot
        /// </summary>
        static void DisplayDiconnectFinchRobot(Finch finchRobot)
        {
            DisplayScreenHeader("Diconnect Finch Robot");

            Console.WriteLine();
            Console.WriteLine("\tReady to disconnect the Finch Robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.Clear();
            DisplayScreenHeader("Diconnect Finch Robot");
            Console.WriteLine();
            Console.WriteLine("\tFinch Robot is now disconnected.");

            DisplayReturnMainMenuPrompt();
        }

        /// <summary>
        /// connect finch robot
        /// </summary>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            bool finchRobotConnected = false;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine();
            Console.WriteLine("\tReady to connect to the Finch Robot. Please be sure to connect the USB cable from the robot to the computer.");
            DisplayContinuePrompt();
            Console.Clear();
            DisplayScreenHeader("Connect Finch Robot");

            finchRobotConnected = finchRobot.connect();

            if (finchRobotConnected)
            {
                finchRobot.setLED(0, 255, 0);
                //finchRobot.noteOn(1500);
                //finchRobot.wait(1000);
                //finchRobot.noteOff();

                Console.WriteLine();
                Console.WriteLine("\tFinch Robot is now connected.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("\tUnable to connect to the Finch Robot.");
            }

            DisplayReturnMainMenuPrompt();

            return finchRobotConnected;
        }
        #endregion

        #region Open/ Close Screens
        /// <summary>
        /// display welcome screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            Console.WriteLine("\tPress any key to get started.");
            Console.ReadKey();
        }

        /// <summary>
        /// display closing screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            Console.WriteLine("\tPress any key to exit.");
            Console.ReadKey();
        }
        #endregion

        #region HELPER METHODS

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Return to main menu prompt
        /// </summary>
        static void DisplayReturnMainMenuPrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to return to the main menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        /// <summary>
        /// valid command choices
        /// </summary>
        static void DisplayValidCommands()
        {
            Console.WriteLine();
            Console.WriteLine("\tValid Commands: ");
            Console.WriteLine();
            Console.WriteLine("\tNONE");
            Console.WriteLine("\tMOVEFORWARD");
            Console.WriteLine("\tMOVEBACKWARD");
            Console.WriteLine("\tSTOPMOTORS");
            Console.WriteLine("\tWAIT");
            Console.WriteLine("\tTURNRIGHT");
            Console.WriteLine("\tTURNLEFT");
            Console.WriteLine("\tLEDON");
            Console.WriteLine("\tLEDOFF");
            Console.WriteLine("\tPLAYSONG");
            Console.WriteLine("\tTEMPERATURE");
            Console.WriteLine("\tDONE");
        }

        #endregion
    }
}
