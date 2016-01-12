# AskMJane

Installing and Restoring DB:

-- Install postgres9.4 and pgadmin 
http://www.enterprisedb.com/products-services-training/pgdownload#windows

-- Install npgsql 2.2.3
http://pgfoundry.org/frs/download.php/3797/Setup_Npgsql-2.2.3.0-r2-net45.exe

-- Install Postgis
http://download.osgeo.org/postgis/windows/pg94/postgis-bundle-pg94x64-setup-2.1.7-1.exe

-- Restore the database 
Open up PgAdmin and connect to your postgres instance
Copy the contents of HarvestGeek.Data/harvestgeek_migrate_initial.sql into PgAdmin command window
execute
