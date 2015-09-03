# SQL Monitor
SQL Server monitor, manages sql server performance, monitor sql server processes and jobs, analyze performance, analyse system, object version control, view executing sql query, kill process / job, object explorer, database shrink/log truncate/backup/detach/attach.


![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/1.png)




It uses linq and requires .net 4.0 (client profile), only support SQL Server 2005/2008/2008R2/2012, not for 2000, sorry :(

The implementation is quick, pretty straight forward, but I try to maintain the logic and make the job done.

There is an article about this project at code project: http://www.codeproject.com/KB/database/sqlmon.aspx

And there is the second article also at code project: http://www.codeproject.com/KB/database/sqlmonitor.aspx



# Why

Ok, I have to admit that I got bored so I just want to make something. It looks like I am reinventing a wheel(duplicating part of SQL Server Management Studio?), mmm, actually, I don't think so. I address something here that do not exist in SQL Server Management Studio at all, at least not in 2012RC0.

 

# Target

Step 1: a real monitor, keep tracking sql server actitivities(sql execution, cpu consumption, disk space etc), alert on customized notifications. 

Step 2: support oracle/mysql/firebird/postgresql etc.

Step 3: accessible on any client (including mobile phones).

 

# Features

* Tracking sql server status, notify query execution and server status
* Version control, tracking table structure, index, trigger, view, function, stored procedure versions
* Server summary
* Analysis, expensive queries, thanks: http://sqlmonitor.codeplex.com/
* Performance Charting
* Object explorer, see object scripts, including tables, view dependencies.
* Process Visualizer, thanks: http://www.codeproject.com/Articles/18378/Organization-Chart-Generator
* Detect dead loop and memory leakage
* Query, table data view
* Database shrink/log truncate/backup/detach/attach
* Syntax Color Highlighting, thanks: http://www.icsharpcode.net/
* Text comparison, thanks: http://www.codeproject.com/KB/recipes/diffengine.aspx
* Object/script search
* Support processes and jobs
* Keep tracking actual running sql query
* Automatically load lan sql server instances
* Auto refresh
* Auto update notification
* Grid grouping, thanks OutlookGrid: http://www.codeproject.com/Articles/14388/OutlookGrid-grouping-and-arranging-items-in-Outloo


# Screen Shots

 

## All New Health Monitor
![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/1.png)


 

## Performance Graph
![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/2.png)


 

## Monitor Multiple Servers / Databases
![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/3.png)


## Popup/Dock Performance Graph
![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/4.png)



## Object Explorer
![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/5.png)


 

## Object Version Control
![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/6.png)


 

## Version Compare
![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/7.png)


 

## Activities
![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/8.png)
 

 

## Process Visualizer
![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/9.png)


## Analysis - Database
![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/10.png)


## Analysis - Execution
![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/11.png)


 

## Analysis Logic Fault
![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/12.png)


 

## Alerts
![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/13.png)


 

## Alerts - Empty Table
![Image of SQL Monitor](https://raw.githubusercontent.com/unruledboy/SQLMonitor/master/images/14.png)