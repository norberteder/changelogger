# CHANGELOGGER #

Create a changelog for a git repository.

## Idea

I need a tool which is able to generate a changelog for a git repository when the versions are tagged (tagname == versionnumber). It should be possible to use this tool by a build process.

## Usage

```
changelogger -r Path\to\repository -e Path\to\exportedfile
```

## Features

* Generate changelog from given repository based on version tags
* Export grouped commits by tags into a markdown file

## Planned features

* Link to ticketing system and group commits to defact from ticketing system
  * Extract defact identifier from commit message
* Be able to generate changelog for a specific version, not only a complete branch

## Contribute

If you like to contribute to this project feel free to add an issue, send a pull request (feature as well as a refactoring) or just send me your opinion.

## License

MIT

