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
  <add key="IdPattern" value="ID-PATTERN"/>
  <add key="TitleFormat" value="TITLE-FORMAT"/>
</appSettings>
```

The `Link`-Key is used to export a link to the ticket. The ticket-id is appended to the given link. 

Define the `IdPattern` to be able to parse the id from the commit message. This is essential for finding an appropriate TFS workitem. As an example you can use `^#[0-9]*` as the ID pattern, so your commit message should look like `#1234: add some new code`.

Use `TitleFormat` to set a format of your choice, e.g. `[{id}] {title}`. The parts must match properties of a TFS workitem. It no `TitleFormat` is given just the `title` is used. You can also use custom fields, e.g. `Fld10111` for the field with the Id `101111`.

When starting `changelogger` add the parameter `-t tfs` to activate the TFS integration. 

## Export

Currently just the export into a markdown file is supported. All commits (or defacts, if a ticketing tool is configured) are grouped by the version tags. You can configure the format version as follows:

```
<appSettings>
  <add key="VersionFormat" value="{major}.{minor}.{build}"/>
</appSettings>
```

There are four options to use: `{major}`, `{minor}`, `{revision}` and `{build}`. Everything that differs won't be replaced.

## Future features

* Link to ticketing system and group commits to defact from ticketing system
  * Extract defact identifier from commit message
* Be able to generate changelog for a specific version, not only a complete branch

## Contribute

If you like to contribute to this probject feel free to add an issue, send a pull request (feature as well as a refactoring) or just send me your opinion. Please send me a short note if you use this tool ([@norberteder](http://www.twitter.com/norberteder "@norberteder")).

## License

MIT

