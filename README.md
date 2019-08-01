# IdentityServer4.AdminUI

## Lets get started
These are a few of the things that are either included within the project, or things that the project uses. 

[Bootstrap](https://getbootstrap.com/) 
Bootstrap is amazing. Bootstrap is what sets up the majority of the interface that we see. We use their librarys and scripts to run most of the pages. This is where we get the designs for the buttons, the navigation bars, and other tools that we used to design the UI. 

[SB Admin](https://startbootstrap.com/templates/sb-admin/) 
This is a bootstrap theme. This theme includes the main table system that appears on most pages of this project. Other features from this template are currently un-used. The tables used in this project also include the required java script files from this template.

[Font Awesome](https://fontawesome.com/) 
This is another open source resource that we're able to use. They have tons of free vector icons that we are able to use for the site. This includes things such as a house for a home page button, or an icon of people for Users. They also allow you to easily edit the icons by changing their size, color, rotation, and animation. They have a list of all of their free icons on their website as well as a neat tool to see the code on the changes. 

[.netcore](https://dotnet.microsoft.com/download) 
This is where the magic happens to allow the project to work cross platform. Currently this was build using .netcore 2.1 

[Settings]
There are some settings that are able to be changed around. Here are a few of them:

-- Security Types -- 
The Identity framework by default only allows for the "Shared Secret" as a secret type. However, you are allowed to design and create custom ones. In the appsettings.json file, there is a section labled "Secret types" This is a pipe seperated list. Just add a '|' and your secret name to this list and every dropdown that uses secret types will be updated!

-- Session Expiration --
This project uses session states to transer information between pages. The session just stores this user specific information on an individual manor. However, this information is set to expire after 20min. by default. Added a setting in the Startup.cs to change this. Currently it is "options.IdleTimeout = TimeSpan.FromMinutes(20);" You can change the number of minutes you would like the sessions to stick around for, There are options to change this time to be based in other units of time. I chose to stick with the default 20min. for this project. 






