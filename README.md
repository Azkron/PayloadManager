@ Hugo Barbachano 2020 / hugobeny@gmail.com

# PowerAssinger webservice

After a payload is received the PowerAssigner webservice performs the following actions :
	
1. If the total avaliable power is equal or lower than the power load it will set all powerplants assingments to pmax.
	
	1.b Else if the total avaliable power is higher than the power load it performs an A* search ( https://en.wikipedia.org/wiki/A*_search_algorithm ) on a dinamically generated graph to find an optimal solution. This algorithm will remain highly performant regardless of the size of the power load or the number of powerplants.
	
	1.c In the marginal case where there is no possible exact solution because of the pmin then it will use the solution that wastes the least energy possible while still having the price into account.

2. After the assingments are calculated it will respond to the POST with the assingments and it will also broadcast both the request and the assingments through a socket connection.

The CO2 is always taken into account for the gas-fired plants.

All requests, responses, actions and errors are logged both in the console and in the file `powerAssingerLogs.txt` at the root of the project. But it would be easy to also store the logs in a database by simply configuring `Serilog` in the Program.cs file.

# Routing
This webservice can receive the payloads through a POST request on `https://localhost:5001/api/powerAssinger` or `http://localhost:5000/api/powerAssinger`. And it also has a socket connection open on `https://localhost:5001/assingments` but for this exercise it only accepts connections comming from `http://localhost:4200` if you need it to accept connections from other urls simply update the line `.WithOrigins("http://localhost:4200")`of Startup.cs.

For testing I used Postman for the payload POST ( `https://localhost:5001/api/powerAssinger` ) and I have develop a simple Angular 9 app, PowerAssingerViewer ( https://github.com/Azkron/PowerAssignerViewer ) that connects to the socket ( `https://localhost:5001/assingments` ) and displays the messages in a fancy way. The repository contains instructions on how to build and run it. 

# Build
This app was created using Visual Studio 2019. If you have problems building make sure your Visual Studio 2019 installation includes `ASP.NET`, `.NET desktop development` and `.NET Core cross platform development`

# .exe
In case you cannot or dont have the time to build the app there is a .exe at `bin\Release\netcoreapp3.1\PowerAssinger.exe`
