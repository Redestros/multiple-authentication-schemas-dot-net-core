# Multitenancy and Multiple authentication schemas in ASP.NET Core

This project is based on multi-tenancy repository by Mukesh Murugan. To understand more multi-tenancy concept you can find detailed infos in 

the following article - https://codewithmukesh.com/blog/multitenancy-in-aspnet-core/

When having a single backend serving users of different apps that may have different identity providers or different authentication settings, we can use
different authentication schemas to fulfill our requirements.

This project represents a Resource Server that validates OpenId tokens form different Identity providers.

We used keycloak as IDP for testing. You can find different realms settings in **Keycloak-realms** folder.

You can Authentication configuring logic in the extension method in Infrastructure project.