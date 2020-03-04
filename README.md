# SyncNoSqlTest

Test project for the following scenario
![](https://i.imgur.com/TylcvD4.png)

1. when user update data, client server will update client side db first then try to update cloud server db throw api
2. after cloud server update, cloud server return history list since last sync time span to now
3. client server receive the history list then will also update the client db by using the histories from cloud
4. Also the client serve schedule a task will run every 5 mins to sync data from cloud server

In this project, I will use **Account** as an example
![](https://i.imgur.com/rPeOm0g.png)

## What to use
- docker with mongbdb installed
- [mongoDB Cloud](https://cloud.mongodb.com/) (of course you can create another mongodb in docker)
- dotnet core 3.1
- swagger

## Steps
we will work on cloud side first
1. create project **cloud**
2. install swagger and mongodb through Nuget
3. create **Account** and **AccountHistory** entities. (Ref to *Data* directory)
4. add db setting in appsetting.json
5. update *Startup.cs* for using mongo db (Ref [here](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-3.1&tabs=visual-studio))
6. update *Startup.cs* for using swagger (Ref [here](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio))
7. create controller with api **Sync** and **Update**
