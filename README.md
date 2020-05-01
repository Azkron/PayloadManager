@ Hugo Barbachano 2020 / hugobeny@gmail.com

# PowerAssinger webservice

This webservice can receive the payloads through a POST request on `https://localhost:5001/api/powerAssinger` or `http://localhost:5000/api/powerAssinger`. And it also has a socket connection open on `https://localhost:5001/assingments` but for this exercise it only accepts connections comming from `http://localhost:4200` if you need it to accept connections from other urls simply update the line `.WithOrigins("http://localhost:4200")`of Startup.cs.

For testing I used Postmant for the payload POST(`https://localhost:5001/api/powerAssinger`) and I have develop a simple Angular 9 app, PowerAssingerViewer (https://github.com/Azkron/PowerAssignerViewer) that connects to the socket(`https://localhost:5001/assingments`) and displays the messages in a fancy way, the repository contains instructions on how to build and run it. 

After a payload is recived the PowerAssigner webservice performs the following actions :
	
	- If the total avaliable power is equal lower than the load it responds by setting all powerplats assingments to pmax.
	
	- Else if the total avaliable power is higher than the load it performs an A* search (https://en.wikipedia.org/wiki/A*_search_algorithm) on a dinamically generated graph to find the most optimal solution.
	
	- In the marginal case where there is no possible exact solution because of the pmin then it will use the solution that wastes the least energy possible while still having the price into account.

The CO2 is always taken into account for the gas-fired plants.

All request, responses, actions and errors are logged both in the console and in the file `powerAssingerLogs.txt` at the root of the project. But it would be easy to also store the logs in a database by simply configuring `Serilog` in the Program.cs file.

# Build
This app was created using Visual Studio 2019, you have problems building make sure your Visual Studio 2019 installation includes `ASP.NET`, `.NET desktop development` and `.NET Core cross platform development`

# .exe
In case you cannot or dont have the time to build the app there is a .exe at `bin\Release\netcoreapp3.1\PowerAssinger.exe`
