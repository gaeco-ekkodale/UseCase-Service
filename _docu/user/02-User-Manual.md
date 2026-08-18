# Introduction

This document guides you through the UseCases module: where the working contexts of a gaeco
platform are defined.

A UseCase is the context that data is viewed and edited from. gaeco never shows simply all the
data — every view and every permission is bound to a UseCase. Energy management and maintenance
planning can address the same building and expose different parts of it: the building is stored
once, and the UseCase decides which section is relevant.

Creating one is the second of the three setup steps the
[start page](https://github.com/gaeco-ekkodale/Homepage) asks for. It has to happen before
access rights can be configured, because rights are always granted *within* a UseCase.

# Prerequisites

- The `UseCase Client` must be running as a plugin inside the Plugin Host.
- The `UseCase Server` and `UseCase Postgres` must be running.
- The following services must also be running:
  - `Keycloak`
  - `Kafka`
  - `MiniO`
  - `PluginHost Service`
  - `AppOrchestrator`

# General Usage

The module is a table of the configured contexts, with a name and a description each.

![The UseCases table listing the configured working contexts.](screenshots/client-screenshot-001.png)

It provides features to:

- [add a UseCase](#adding-a-usecase)
- [edit an existing one](#editing-a-usecase)
- [search, filter and arrange the table](#finding-and-arranging)

# Adding a UseCase

On a platform that already has UseCases, the control is in the table toolbar. On an empty
platform the module offers a prominent **Create UseCase** button instead.

![Add UseCase opens the creation dialog.](screenshots/client-screenshot-002.png)

A dialog asks for a name and a description. Both are required.

![An empty UseCase dialog, asking for a name and a description.](screenshots/client-screenshot-003.png)

## Choosing a Good Name

This is worth a moment's thought, because the name is what everyone chooses from later, in every
other module's UseCase selector.

Name a UseCase after **the task it serves**, not after a department or a team — "Energy
monitoring", "Maintenance planning", "Space management". A UseCase called "Facility Management
Team" says nothing about what the context is for, and stops being accurate the moment the team is
renamed.

![A completed form — named after the task the context serves.](screenshots/client-screenshot-004.png)

**Save** adds it to the table.

# Editing a UseCase

Editing happens in place. Double-click a cell to change it; the change applies when the field
loses focus — press `Enter`, `Tab`, or click outside the cell.

![Double-clicking a cell edits it in place; the change applies on blur.](screenshots/client-screenshot-005.png)

Renaming a UseCase does not affect the data or the permissions attached to it: those refer to its
identifier, not its name.

# Finding and Arranging

The toolbar holds the table controls.

![The toolbar: search, add, column filters, columns, density, full screen.](screenshots/client-screenshot-006.png)

| # | Control | Purpose |
| --- | --- | --- |
| 1 | Search | Filter across all columns |
| 2 | Add UseCase | Create a new context |
| 3 | Column filters | Filter each column separately |
| 4 | Columns | Show or hide individual columns |
| 5 | Density | Change the row height |
| 6 | Full screen | Maximise the table in the viewport |

## Search

The search field filters the table as you type. Matching text is highlighted; rows that do not
match are hidden.

![Searching filters the table as you type.](screenshots/client-screenshot-007.png)

## Column Filters

For more precise results, each column can be filtered on its own, and the filters combine — a
name filter and a description filter apply together. The general search can be combined with
them as well.

![Per-column filters, which can be combined across columns.](screenshots/client-screenshot-008.png)

## Showing and Hiding Columns

Useful mainly for hiding the description, which is long.

![Choosing which columns the table shows.](screenshots/client-screenshot-009.png)

## Density

![Density changes the row height.](screenshots/client-screenshot-010.png)

## Full Screen

![Full screen maximises the table in the viewport.](screenshots/client-screenshot-011.png)

## Pagination

The table shows 10 rows per page by default, adjustable through the dropdown. Beside it the
current range and total are shown, and the arrows move between pages.

![Pagination: rows per page, the current range, and page navigation.](screenshots/client-screenshot-012.png)

# The Built-in Tour

The help button replays the module's own explanation at any time.

![The tour explains how to name a UseCase well.](screenshots/client-screenshot-013.png)

# Deleting a UseCase

Deleting a UseCase is deliberately not offered in this module. A UseCase is referenced by the
access rights configured for it and by the data created under it, so removing one is not a local
operation. Retire a context by taking its permissions away in the
[Access Rights module](https://github.com/gaeco-ekkodale/AccessService) instead.

# For Developers

The UseCase Service publishes its changes via Kafka, so other services can react to a UseCase
being created or renamed. See the [developer documentation](../developer/01-Concepts.md).

# Related Documentation

- The deployment repository's user guide — all three setup steps in order
- [Access Rights](https://github.com/gaeco-ekkodale/AccessService) — the next setup step
- [Instances](https://github.com/gaeco-ekkodale/InstanceService) — where data is created
  within a UseCase
