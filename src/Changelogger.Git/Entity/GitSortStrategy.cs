using System;

namespace Changelogger.Git.Entity
{
    /// <summary>
    /// Currently the same values as <see cref="GitLib2Sharp.CommitSortStrategies"/>
    /// </summary>
    [Flags]
    public enum GitSortStrategy
    {
        //
        // Summary:
        //     Sort the commits in no particular ordering; this sorting is arbitrary, implementation-specific
        //     and subject to change at any time.
        None = 0,
        //
        // Summary:
        //     Sort the commits in topological order (parents before children); this sorting
        //     mode can be combined with time sorting.
        Topological = 1,
        //
        // Summary:
        //     Sort the commits by commit time; this sorting mode can be combined with topological
        //     sorting.
        Time = 2,
        //
        // Summary:
        //     Iterate through the commits in reverse order; this sorting mode can be combined
        //     with any of the above.
        Reverse = 4
    }
}
