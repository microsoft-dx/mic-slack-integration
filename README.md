
# Microsoft Innovation Center Slack Integration
###Slack Integration for the Microsoft Innovation Center Team using **Asp.Net WebApi**, **SignalR**, **Autofac** and **Azure SQL**


###Introduction
Purpose of the project

- people that are not members of our Slack team to be able to post messages in a dedicated channel

- team members to be able to reply to messages using a **[slash command](https://api.slack.com/slash-commands)**


###Implementation

We broke down the project in two parts:

- posting messages to a Slack  channel from a public  web page

- posting messages from Slack to the public web page


![mic-slack-integration-architecture](https://kmnhpa.bn.files.1drv.com/y4mN0IuCjJuF_jGvI6HJdQYneb8N34UfGjvfY-x0Frw1gPg3ho70Gl1iNxb_K5bOFQiqQ7ZidJak2TXuXh7vUR1PfmAX-UgDd21ylcrC2-sQVr15QL1UJa-ZBUzaCjohm-nFj6tPIlDZ3kR_cEOT5R426AW5_6pKrh4_JzwwhNiQYn9vXoevNjpMUh5dkEGn3sfb_FjHRB3uWFO1sqTG9KM7Q?width=1280&height=720&cropmode=none)


###Posting messages to Slack from a public web page

This part of the project was implemented using  **[Incoming WebHooks](https://api.slack.com/incoming-webhooks)**

We [set up a new Incoming WebHook](https://my.slack.com/services/new/incoming-webhook/) for a specific channel and received a WebHook URL that we used to post the messages from the public web page.

The message post functionality is done by `SlackClient.cs` , with `SlackMessage` as message model.

###Posting messages from Slack to the public web page

This part was implemented using  **[Slash Commands](https://api.slack.com/slash-commands)**.

When the message starts with `/command-name`, a post is created to a `URL` that you can set up in the [Slack Integration for a slash command console](https://my.slack.com/services/new/slash-commands), with `command-name` also set up in the console. 



###Implementation

Both scenarios are implemented inside a WebApi controller, `SlackController` with two methods: `PostMessageToSlack` and `PostMessageToWeb`.


The final goal is for public messages to appear both in the web page and the specific Slack channel.

So,  both methods in the controller post the messages to the Slack channel and to the public web application in real-time (the real-time functionality for the web page is achieved using SignalR)

Basically, each controller method does the following:

`SlackClient.PostMessage(message);`

`ConnectionManager.GetHubContext<SlackHub>().Clients.All.addMessage(message.UserName, message.Text);`

`SlackMessageStore.SaveMessage(message);`

(Since we use Autofac for Dependency Injection, there is no need for the `GlobalHost`global object to retrieve the hub context - see [the Autofac Docs](http://autofac.readthedocs.org/en/latest/integration/signalr.html) or [this repo](https://github.com/radu-matei/signalr-dependency-injection-autofac) for an example and for more information on using SignalR and Autofac  )


The `PostMessage`method from `SlackClient` basically does an HTTP Post to the `Uri` provided by the Slack Incoming WebHook.

The `SlackMessageStore` has the message store functionality that stores the messages in an Azure SQL Database.

###Building the project

In order to build the project, you need to add two `.config` files in the root of the API project:

- `appSettings.config`

    ```<?xml version="1.0" encoding="utf-8" ?> 
      <appSettings> 
        <add key="SlackWebHookUri" value={web_hook_uri}/>
        <add key="SlackCommandToken" value={command_token}/>
      </appSettings>```

- `connectionStrings.config`

`<?xml version="1.0" encoding="utf-8" ?> 
<connectionStrings> 
  <add name="SlackDbConnectionString" connectionString = {connection_string} />
</connectionStrings>
`

If you don't need it, you can exclude the message store and the SQL Database, but you must remove the dependencies for `SlackMessageStore` and for `SlackDbContext`, and the not register `SlackDbContext` with the builder. 

`            builder.RegisterType<SlackDbContext>()
                .WithParameter("connectionString", ConfigurationManager.ConnectionStrings["SlackDbConnectionString"].ConnectionString)
                .InstancePerRequest();`


###Next steps

- create a decent UI for the public web page

- create support for automatic responses

- add support for attachments in messages

###Conclusion

This repo can be used for integrating Slack with any service that has a REST or .NET API, since it implements both Slash Commands and Incoming WebHooks.
