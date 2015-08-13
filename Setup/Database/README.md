Database Setup
=========
The application database can be built using one of the database build folders. Each build folder contains an initial file "YYYYMMDD_initial.sql" as well as none or more update scripts (YYYYMMDD_update.sql).  If you are building a fresh local copy, you want to start your database setup using the most recent build folder.

Steps to Create Database
-----------
1. Create a new Database on your database server (we use Brewgr_DEV) 
2. Locate your DB Build folder (instructions above)
3. Run the "YYYYMMDD_initial.sql" file to create all of the tables, views, etc necessary to run the application.
4. If there are any "YYYYMMDD_update.sql" files in the Build folder, run each of them in order by date.  This will apply any updates that have been made since the initial build was created.
5. You will need to create a database user, either SQL Auth, or Windows Auth, depending upon what works best for your setup.  (Personally, I recommend running SQL Server Express locally and using Windows Auth)
6. Make sure your new DB user has read/write/execute access on the database ... for dev purposes, granting them dbowner is easiest. 
