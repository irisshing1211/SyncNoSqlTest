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
1. 
