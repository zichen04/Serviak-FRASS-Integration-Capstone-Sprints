# Sprint 2 Report (October 11–November 8, 2026)

**Team:** WSU CptS Capstone Team 14 — Ziang Chen, Meshach Erisca, Tomas Stoker  
**Client/sponsor:** D&D Larix, LLC / Dr. Bill Schlosser  
**Status:** We still need the video link.

## YouTube link of Sprint 2 Video

We have not finished the sprint video yet. We will add the link here when it is ready.

## What's New (User Facing)

We did not finish a feature that users can access in FRASS this sprint. We got FVS-calling code working on Serviak and ran a Year Zero case using FVS-compatible SQLite databases on the server. Those are important internal steps, but FRASS cannot yet take live data through FVS and show the results to a user. A lot of the pipeline has to work together before there is something meaningful to show in the application.

## Work Summary (Developer Facing)

Our [October 15](MoM/MoM_2026-10-15.md), [October 22](MoM/MoM_2026-10-22.md), [October 29](MoM/MoM_2026-10-29.md), and [November 4](MoM/MoM_2026-11-04.md) meetings helped us work out how Cruiser and GIS data should reach FVS, what a stand-level run needs, and how to keep tree records traceable when results come back to FRASS. Meshach revised his C# and R run scripts and used FVS-compatible SQLite databases on Serviak to get a Year Zero run working. The more complex baseline run took longer because of rFVS limitations, FVS input requirements, and a bug caused by running R scripts from Visual Studio folders. Ziang wrote three C# components to read Cruiser and FVS tree data and prepare a future Flex Taper handoff, but they still need real FVS output and a live FRASS connection. Tomas created an initial data-preparation skeleton and an execution-sequence outline for building and checking an FVS run. We made progress on the separate pieces of the pipeline, but the data mapping, validation, and end-to-end connection are still in progress.

## Unfinished Work

- Ziang's [issue #14](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/14) is still in progress. His code can prepare tree values from two SQLite sources, but it does not call Flex Taper or read from FRASS's live data source. He needs to test it with a real FVS output database, check that `FVS_TreeList` has the expected columns and Year Zero rows, and confirm whether the FVS tree IDs match our source tree IDs one to one. We also need to decide which cubic-foot field Flex Taper should use. His current Cruiser reader gets stump height from the Cruiser database, while our newer understanding is that stump height is entered in FRASS, so that source needs to change.
- Meshach [issue #13](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/13) Did not finish the more complex baseline run or the C# and R unit tests he planned. The rFVS scripts and FVS input requirements took more work than expected, and he also had to fix an R working-directory problem in Visual Studio. Next sprint, he plans to add tests, revise code where tests expose errors, and work with us to check FVS input and output.
- Tomas [issue #12](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/12) Input preperation and run-sequence work is still a skeleton. He needs the concrete FRASS database format to map its fields into FVS input and then validate both databases while preserving the FRASS data for later checks. Unknown database details and missed meetings slowed that work. He expects to complete initialization before adding full validation.


## Code Files for Review

- `frass/FRASS.FVS/Services/CruiseTreeReader.cs` — reads tree IDs and stump heights for one stand from a supplied Cruiser SQLite database. Ziang needs to change where it gets stump height if FRASS is the source.
- `frass/FRASS.FVS/Services/FvsTreeListReader.cs` — reads tree results for one case, stand, and year from an FVS SQLite database. Ziang needs to check its expected table and Year Zero rows against a real run.
- `frass/FRASS.FVS/Services/FvsFlexTaperHandoff.cs` — matches the two sets of tree records and prepares values for a later Flex Taper call. We still need to confirm the ID matching and volume choice.

Ziang's [commit](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/commit/cce28354daa5ab6349e917f2f7a4a03af1f3e050)

- `frass/FRASS.FVS/RFVS/FVSKeyFile.cs` — launches R to create a keyfile from stand IDs, input/output database paths, and cycle count. It also has a Year Zero helper that requests zero cycles.
- `frass/FRASS.FVS/RFVS/FVSKeyFileBuilder.r` — calls rFVS::fvsMakeKeyFile with those settings. It accepts optional additional FVS keywords.
- `frass/FRASS.FVS/RFVS/FVSRunner.cs` — launches the R run script, captures its output and errors, and reports a failed process. It does not yet return the output database path.
- `frass/FRASS.FVS/RFVS/RunFVS.r` — loads the selected FVS model through rFVS, supplies the keyfile, and calls fvsRun().

Meshach's [commit](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/tree/d2d619f9ee6133fea2e86e756203c23e29c86ac8/frass/FRASS.FVS/RFVS/RFVS%20Serviak%20Testing)

## Retrospective Summary

**What went well:** We made reasonable progress with the time we had. Ziang had four job interviews during the sprint on top of other assignments and exams, while the team continued moving the technical work forward. Dr. Bill was happy that we got FVS-calling code working on Serviak. Meshach also got a Year Zero run working with the server's FVS-compatible SQLite databases. The client meetings gave us a clearer picture of the data we need to preserve and return to FRASS.

**What we would improve:** We need to upload meeting notes sooner and use the Kanban board more consistently. Some of our issues are too broad to show what each person is doing or where work is stuck. As team lead, Ziang needs to check in more often. He assumed teammates had what they needed and only learned during a meeting that someone could not work on a task because they lacked server access. We also need to identify unknown data formats and testing dependencies earlier so they do not stall the next implementation step.

**Changes planned for Sprint 3:** Ziang will check in with the team more regularly and ask directly about access, dependencies, and blockers. We will break work into smaller issues with clear owners and acceptance criteria and keep the board current. We will coordinate C# testing with the other teams' planned NUnit approach and use testthat for R; Meshach plans to add tests and revise the run code based on what they uncover. Tomas plans to map a real FRASS database into the initialization step and continue validation. Ziang needs to test the handoff against real FVS output before wiring it into FRASS and Flex Taper.
