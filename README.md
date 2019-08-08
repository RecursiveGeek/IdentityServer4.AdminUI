# IdentityServer4.AdminUI

IdentityServer4.AdminUI is an .NetCore web application designed to help manage an Identity4 server.

A diagram of the database style this application was initially designed for can be found [here](https://id4withclients.readthedocs.io/en/latest/id4/ID4Database/DatabaseDiagramID4.html#client-app-related-tables).

  
  

## Lets get started

These are a few of the things that are either included within the project, or things that the project uses.

  

[Bootstrap](https://getbootstrap.com/)

Boostrap is the main front-end library. Bootstrap helps with the designing of the User Interface. Bootstrap provides libraries to help with the navigation bar, the buttons, and CSS. More information on how to install, bootstrap documentation, and more can be found on the bootstrap website.

Bootstrap also allows for use of it's themes such as the one we use below.

  

[SB Admin](https://startbootstrap.com/templates/sb-admin/)

SB Admin is a free bootstrap theme. This theme has many neat features, however this project only uses the table. This table allows for multiple pages to be displayed with a custom number of items per page.This also includes a quick real time search feature on each page.

Note: These functionalities of this table are java script reliant.

  

[Font Awesome](https://fontawesome.com/)

Font Awesome is the source of the vector icons. It is a free and opensource* project that gives access to thousands of free and css customization icons. This project uses some of these icons as navigation buttons. Alternate Icons and customization options can be found on the font awesome website.

  

*There is a paid version available, this project only uses options that are available for free.

  

[.Net Core](https://dotnet.microsoft.com/download)

This project was built using .NetCore v2.1. This is the main framework of the project. Using .Net Core allows this project to be applicable across all platforms. This means the application should work on all browsers and operating systems, including mobile.

  

  

[Identity Model](https://github.com/IdentityModel/IdentityModel)

This is libary to assist with using OAuth 2.0 for the application. This is mainly used for encryption to match the server.

  

[Safe Storage](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.1&tabs=windows)

This project supports the use of appsettings.json as well as Safe Storage. This is accessed at:

      \AppData\Roaming\Microsoft\UserSecrets\8678fff0-e8bb-4455-ac1c-62f234549f0f\secrets.json


This file has the same format as the included appsettings.json file, with the appropriate value information filled in for each data item.



## Settings
After setting up the above features, there are custom settings for everyone. Most of these can be found in the appsettings.json* file (secrets.json when using Safe Storage). Below are the features that can be changed based upon individual needs. 

*note: Settings changed in the appsettings.json will not require an application restart to be applied. 

* Secret Types. 
	Identity Server 4 supports "Shared Secret" as the initial valid secret type. However, Users are free to make custom options as they wish. This is in a list separated by a delimiter. The delimiter option is also editable above, but this allows additional secret types to be added to the list easily. 
	
* Grant Types
	 The list that is seen in the appsettings.json is the list from the Identity documentation found [here]([http://docs.identityserver.io/en/latest/topics/grant_types.html](http://docs.identityserver.io/en/latest/topics/grant_types.html)). Like Secret Types, this is a delimiter list. Additional options are able to be added by changing this field. 
	 
* CopyRight
This is the copyright date that appears in the bottom corner of the program.

* Error404Url
This is the url location where the program will take the User if the page can not be found. If you would like a custom page or different location this is where it can be changed. 

* Secret Names
These are the session names. This application uses [Sessions]([https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-2.1](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-2.1)). This requires cookies to be in use for some features of the app to function properly. This is what allows most of the information to be passed around the pages. The names on this list is the variable name that those variables are assigned to. Being places here they are in a centralized location to keep the application consistent. You can change these if you wish. 

* Session Expiration.  
This setting is found in "Startup.cs". This setting is how long the cookies for the sessions mentioned above will be active for. The setting to change this is:
 `"options.IdleTimeout = TimeSpan.FromMinutes(20);" ` 
 Currently the length of this is set to 20 minutes. This is what the natural default setting is. This can be changed to other times if the session needs to last longer or shorter. 
 
 
  

~ end ~
