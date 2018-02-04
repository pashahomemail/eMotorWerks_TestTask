# eMotorWerks_TestTask
Test task for eMotorWerks

1. Set up your connection string for SQLEXPRESS
    <!--<add name="ApplicationConnectionString" connectionString="Data Source=.\sqlexpress; AttachDbFileName=|DataDirectory|\DB.mdf; Integrated Security=True; MultipleActiveResultSets=True; User Instance=True" providerName="System.Data.SqlClient"/>-->
    <!--<add name="ApplicationConnectionString" connectionString="Data Source=.\sqlexpress2; AttachDbFileName=|DataDirectory|\DB.mdf; Integrated Security=True; MultipleActiveResultSets=True; User Instance=True" providerName="System.Data.SqlClient"/>-->
    <add name="ApplicationConnectionString" connectionString="Data Source=.\sqlexpress2; Initial Catalog=TestDB; Integrated Security=SSPI" providerName="System.Data.SqlClient" /></connectionStrings>
    
    uncommit what u need or create another connection string (care about this place Data Source=.\sqlexpress)
2. Create init migration (Package Manager Console) from command add-migraion init
3. Update migration (db will create now after this command) update-database
4. Overview api implementation in index.html.

Used technologies: 
IDE: Visual Studio 2015
Back-end: .Net, C#, WebAPI, EntityFramework 
Front-end: Vue.js, HTML5, CSS, JS, Bootstrap
