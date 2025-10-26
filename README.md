# Quiz

## Introduction
A dynamic, cross-platform solution for knowledge assessment. This quiz generator features both a responsive web portal and a native desktop application, providing universal access. Users receive a fresh, randomly generated quiz from a question database every time. After completion the user gets a performance report based on answers they give.

## Feautres
### Core feautres
  - Random question selection
  - Calculation of statistics and scoring
  - State management and validation
  - Multi platform support (web portal + desktop app)
### Data Handling and Security
  - SQL data base support
  - 100% code coverage (for bussiness logic in Aplication Layer)
  - API Rate limiting and health checks
  - Vulnerability checking assured by Snyk (SAST via CI pipeline from Github)
### Ways to interact
  - Web
  - Desktop app
  - Api

## Tehnical Aspects
### Architecture
This project is build using an updated version of Layered Architecture.
The Persistence Layer has been moved higher up in the hierarchy, between Application Layer and Presentation Layer.
### Database design
#### Overview
This project is designed to work with SQL databases. Although it is possible to use any storage method with minimal modification due to use of Repository Pattern.
Only one table is needed for the entire app, the "Question" table which needs to have the following collumns:
#### ID (int, primary key)
      - it self explanatory
#### Text (nvarchar)
      - Stores the question itself
#### TextSecond (nvarchar, nullable)
      - Store the second part of some types of questions
      - Exemple: "If your language supports ______, you don t have to manage dealocation of memory." The second part comes after "_____"
#### Options (nvarchar)
      - Stores the options for that questions
#### CorrectOption (nvarchar)
      - Stores the indices of correct options (first option is 0 and the last is size-1)
#### Type (int)
      - Stores the type of the question.
      - There are 2 values that this field can get:
        - 0 -> Choice
          - If the question is simple question (e.g. "What day is today?" / "Which of the following plants can survive in the dessert?", etc.)
        - 1 -> Complete Empty Space
          - Is used if the question options need to match the Question Text to be correct (e.g. "_____ is the 3rd day of the week)
