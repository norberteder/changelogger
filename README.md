# CHANGELOGGER #

Create a changelog for a git repository.

## Idea

I need a tool which is able to generate a changelog for a git repository when the versions are tagged (tagname == versionnumber). It should be possible to use this tool by a build process.

## Usage

```
changelogger -r Path\to\repository -e Path\to\exportedfile
```

More parameters:

* `-t [ticketingshortcut]`: Ticketing system - TFS is supported currently (see below)
* `-l`: Use this parameter to enable links to the ticketing system (export)
* `-v`: Verbose mode -> output of detailed information to the standard output
* `--tag [tagname]`: Tag for which a changelog should be generated

## Features

* Generate changelog from given repository based on (all) version tags
* Generate changelog for a single tag
* Export grouped commits by tags into a markdown file
* TFS Integration

## TFS Integration

Add the following lines to the `app.config`:

```
<configSections>
  <section name="TicketingTypeSection" type="Changelogger.Shared.Ticketing.Configuration.TicketingTypeSection, Changelogger.Shared"/>
</configSections>
<TicketingTypeSection>
  <types>
    <clear/>
    <add key="tfs" value="Changelogger.Ticketing.TFS.TFSTicketingIntegration, Changelogger.Ticketing.TFS"/>
  </types>
</TicketingTypeSection>
```

Furthermore you have to add a setting which defines the TFS collection you want to connect:

```
<appSettings>
  <add key="Collection" value="URL-TO-TFS-COLLECTION"/>
  <add key="Link" value="SET-TICKET-LINK"/>
</appSettings>
```

The `Link`-Key is used to export a link to the ticket. The ticket-id is appended to the given link.

When starting `changelogger` add the parameter `-t tfs` to activate the TFS integration. 

## Future features

* Link to ticketing system and group commits to defact from ticketing system
  * Extract defact identifier from commit message
* Be able to generate changelog for a specific version, not only a complete branch

## Contribute

If you like to contribute to this probject feel free to add an issue, send a pull request (feature as well as a refactoring) or just send me your opinion. Please send me a short note if you use this tool ([@norberteder](http://www.twitter.com/norberteder "@norberteder")).

## License

MIT

