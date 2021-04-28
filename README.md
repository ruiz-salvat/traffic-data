# TrafficDataBackEndAPI

* Back-end API which provides traffic data from the Dutch roads and highways taken from the National Data Warehouse (NDW)

* Built using .Net core 3.1 framework

* Database: Postgres

## Remarks 

* The data stored in the database belongs to one of the multiple measurement points located in a road or highway

* The data is taken in intervals of x minutes (where x is a value defined at the configuration)

* The measurement points retrieved are located in a certain area specified at the configuration

* The data is kept in the database for a x days (where x is a value defined at the configuration), then is deleted

* The flow values are the number of measured vehicles per hour 

* The speed values are averages of the measured vehicle speeds

> For more information check the wiki