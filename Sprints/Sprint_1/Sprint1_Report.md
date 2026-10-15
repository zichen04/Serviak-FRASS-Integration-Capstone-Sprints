# Sprint 1 Report — FRASS–FVS Integration (TODO: sprint start and end dates)

**Team:** WSU CptS Capstone Team 14 — Ziang Chen, Meshach Erisca, Tomas Stoker  
**Client/sponsor:** D&D Larix, LLC / Dr. Bill Schlosser

## YouTube link of Sprint 1 Video

https://youtu.be/pdK512Stfqs

## What's New (User Facing)

No new FRASS user-facing functionality was implemented by this team during Sprint 1. The team did complete a first local FVS run through R without using the FVS Online browser interface ([issue #9](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/9)). This demonstrates a technical step toward the integration, but FRASS does not yet initiate the run or import its results.

## Work Summary (Developer Facing)

We used technical meetings and the project's requirements assignment to map a proposed path from approved FRASS stand data to FVS input, execution through R/rFVS, validated output, and traceable results in FRASS. We documented a base-run use case, three user stories, eight functional requirements, four nonfunctional requirements, and their relationships. The team [set up and launched the inherited application](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/4) and completed a [first local FVS run from R](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/9) without the browser interface. The exact commands, inputs, and outputs still need to be recorded in this report. The main unresolved design question is how the existing FRASS SQLite export connects to R-driven FVS execution and returns results to FRASS.

### Sprint objectives and requirements gathering

- [September 17 technical meeting](MoM/MoM_2026-09-17.md): Dr. Nick Crookston and Dr. Bill Schlosser helped the team understand FVS input databases, R/FVS Online, regional simulation libraries, baseline runs, and the proposed return of results to FRASS.
- [October 1 technical meeting](MoM/MoM_2026-10-01.md): the team discussed the Forest Service installation, user workspaces, command-line/R and browser access, user-supplied data, run identifiers, and possible output reports.
- The requirements assignment records **UC-01: Initiate Base Run** and three user stories: **US-01** initiate a base run, **US-02** run an alternative scenario, and **US-03** review a projection and its evidence. These are specifications, not completed product stories.


| Requirement | Proposed behavior | Related story |
| --- | --- | --- |
| FR-01 | Preserve analytical lineage, including source and GIS site evidence. | US-03 |
| FR-02 | Prepare FVS inputs and fill or flag required fields. | US-01 |
| FR-03 | Validate FVS outputs before using them. | US-01 |
| FR-04 | Maintain a clear FRASS/FVS software boundary. | US-01 |
| FR-05 | Keep candidate states temporary until selected. | US-02 |
| FR-06 | Support a Year Zero baseline. | US-01, US-02 |
| FR-07 | Handle errors and discard invalid states throughout the pipeline. | US-01 |
| FR-08 | Support distinguishable alternative scenarios from one baseline. | US-02 |

The assignment also lists reliability, performance without storing every candidate, user-readable errors, and maintainable data contracts as nonfunctional requirements. The October 1 discussion raised user-supplied data and FVS reports as possible additional needs; their scope and acceptance criteria still need client confirmation.

The proposed UC-01 activity is: request a projection → assemble an approved stand and settings → validate input → execute FVS → validate output → return results with lineage. Missing input or failed execution/output validation should stop the run and explain the problem.

### Installation and evaluation of the existing system

The board records that all three team members [set up and launched the inherited application](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/4). The team also completed a [local FVS run through R](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/9) without the browser interface. For the demo, record the machine and tool versions, R commands, input data, output evidence, and any failures or limitations. The code findings below come from a **static repository review** and should not be presented as runtime test results. The inherited `frass/FRASS.sln` contains the FRASS web application. `frass/FRASS.FVS/Services/FvsDatabaseBuilder.cs` builds SQLite `FVS_StandInit` and `FVS_TreeInit` input tables; `frass/FRASS/FvsDatabaseGenerator.aspx.cs` and `DownloadFvsDatabase.ashx` provide a related page and download path. The [local setup guide](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/blob/main/frass/docker/README.md) describes Visual Studio, Docker services, required data files, and startup commands.


### Kanban board, contributions, and learning

**[Capstone project board](https://github.com/users/zichen04/projects/2/views/1):** The board contains retrospectively entered Sprint 1 onboarding, documentation, setup, and R-learning issues, plus FVS run investigations. Show the board and issue descriptions in the video. Keep US-01 through US-03 open until working software meets their acceptance criteria.

| Member | Sprint 1 contribution and issue evidence | Skill or resource studied and what was learned |
| --- | --- | --- |
| Ziang Chen | [Project description #3](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/3): sections 1.1 and 1.3. [Requirements #8](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/8): introduction, team, nonfunctional requirements, user stories, traceability matrix. Completed [Serviak remote access #5](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/5); assigned to [local sample FVS run #11](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/11), currently in progress. | Participated in [R familiarization #6](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/6). Youtube tutorial. |
| Meshach Erisca | [Project description #3](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/3): section 1.2. [Requirements #8](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/8): system requirements specification, standards, system evolution. Completed a [first local FVS run from R #9](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/9) without the browser interface. | Participated in [R familiarization #6](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/6). Youtube tutorial. |
| Tomas Stoker | [Project description #3](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/3): sections 1.3 and 1.4. [Requirements #8](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/8): use cases and functional requirements. No separate in-progress issue was assigned on the board at review time. | Participated in [R familiarization #6](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/6). Youtube tutorial. |

All three members were assigned to [onboarding #2](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/2), [application setup #4](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/4), and [R familiarization #6](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/6). All contributed to the project description's glossary and references as needed. Collective learning topics identified in the meetings include FVS input/output databases, `fvsOL` and `rFVS`, regional FVS binaries, R-driven runs, and FRASS's C#/SQL Server environment. The board does not specify what each person learned from a particular resource, so those details still need individual confirmation.

### Responsible use of generative AI

AI assistance was largely not used for this Sprint. In the times it was used, it was used as a tool for quickly searching up concepts to gain a better understanding.

## Unfinished Work

- Confirm the input/output units, identifiers, variant selection, missing-value rules, and how the inherited SQLite generator fits the R/rFVS path. (Meeting next week with other teams and client)
- Plan and implement the first small integration slice in a later sprint; no new code is reported for Sprint 1.

## Completed Issues/User Stories

**Product user stories completed:** None so far, no implementation yet.

**Sprint documentation/evaluation issues recorded as completed on the board:**

- [#2 Read onboarding documents](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/2) — Ziang, Meshach, and Tomas.
- [#3 Project Description document](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/3) — all three; section assignments are listed in the issue description and contribution table above.
- [#4 Set up and launch application](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/4) — all three; detailed setup evidence is still needed in this report.
- [#5 Remote access to Serviak](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/5) — Ziang.
- [#6 Research and familiarize with R](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/6) — all three; individual resources and learning results still need to be recorded.
- [#8 Requirements and Specifications section document](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/8) — all three; section assignments are listed in the issue description and contribution table above.
- [#9 First FVS run using R and DLLs](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/9) — the local run succeeded. Record its commands and results, then confirm the issue's final status on the board.
  
These are documentation, access, and learning tasks, not implemented product user stories. The inspected issues have no linked pull requests or milestones; add any later review or submission links that the team actually creates.

**Technical milestone:** The team completed a [first local FVS run through R](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/9). Add its commands, input and output evidence, and issue closeout status before final submission.

## Incomplete Issues/User Stories

- [#11 Reproduce sample FVS run locally](https://github.com/zichen04/Serviak-FRASS-Integration-Capstone/issues/11) — assigned to Ziang; open and in progress. Downloading the USFS package and using its interface to understand a sample run.



## Code Files for Review

No code files were actively developed by this team during Sprint 1. The inherited files listed in the existing-system section were inspected for planning and should not be presented as Sprint 1 code changes. If code was actually changed during this sprint, replace this statement with links to the relevant files and pull requests.

## Retrospective Summary

**What went well:** The two documented technical meetings provided a concrete model of the FVS data and run workflow. The requirements assignment establishes a base-run use case, user stories, and a traceability starting point. The inherited repository offers an input-database component and setup documentation.

**What we would improve:** The team still needs a reproducible setup result and a confirmed integration boundary. Meeting notes were reconstructed after the fact, so the team should review and circulate minutes sooner. TODO: add the team's own retrospective observations.

**Changes planned for Sprint 2:** Confirm the client-approved data contract and report needs; document and inspect the successful R-driven FVS run; finish local interface exploration; and implement one testable path from FRASS-prepared input toward a baseline result. TODO: revise this plan after the team's retrospective and client feedback.

## Sources to verify before submission

- Team 14, *FVS Integration and Automation Framework: Project Requirements and Specifications* (supplied assignment draft; cover date requires correction).
- The September 17 and October 1 meeting minutes linked above, both reconstructed from team notes and pending attendee review.
- The inherited repository files and setup guide named in the existing-system section.
