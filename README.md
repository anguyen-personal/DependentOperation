# Dependent Operation

A program to sort through the order of items to process based on dependent items.

## Description

A person needs to figure out which order his/her clothes need to be put on. 
The person creates a file that contains the dependencies.
 
This input is a declared array of dependencies with the [0] index being the dependency and the [1] index being the item. 
 
A simple input would be:
 
var input = new string[,]
{
        //dependency    //item
        {"t-shirt",             "dress shirt"},
        {"dress shirt", "pants"},
        {"dress shirt", "suit jacket"},
        {"tie",                           "suit jacket"},
        {"pants",     "suit jacket"},
        {"belt",         "suit jacket"},
        {"suit jacket", "overcoat"},
        {"dress shirt", "tie"},
        {"suit jacket", "sun glasses"},
        {"sun glasses", "overcoat"},
        {"left sock",                "pants"},
        {"pants",     "belt"},
        {"suit jacket", "left shoe"},
        {"suit jacket", "right shoe"},
        {"left shoe",               "overcoat"},
        {"right sock",             "pants"},
        {"right shoe",            "overcoat"},
        {"t-shirt",    "suit jacket"}
};
 
In this example, it shows that they must put on their left sock before their pants. Also, 
they must put on their pants before their belt.
 
The application provides the order that each object needs to be put on.
 
The output is a line-delimited list of objects. If there are multiple objects that
can be done at the same time, list each object on the same line, alphabetically 
sorted, comma separated.
 
Therefore, the output for this sample file would be:
 
left sock,right sock, t-shirt
dress shirt
pants, tie
belt
suit jacket
left shoe, right shoe, sun glasses
overcoat

## Usage
---
```console
DependentOperation.exe {filepath} {hasHeader}
```

Examples:
```console
DependentOperation.exe c:\items.csv true
DependentOperation.exe c:\items.csv y
```

Execution without arguments will prompt for filepath and hasHeader.

## CSV Sample files

### with no header
t-shirt,dress shirt
dress shirt,pants
dress shirt,suit jacket
tie,suit jacket
pants,suit jacket
belt,suit jacket
suit jacket,overcoat
dress shirt,tie
suit jacket,sun glasses
sun glasses,overcoat
left sock,pants
pants,belt
suit jacket,left shoe
suit jacket,right shoe
left shoe,overcoat
right sock,pants
right shoe,overcoat
t-shirt,suit jacket

### with header 
Dependency,Item             <-- NOTE: The header must be labeled Dependency and Item
t-shirt,dress shirt
dress shirt,pants
dress shirt,suit jacket
tie,suit jacket
pants,suit jacket
belt,suit jacket
suit jacket,overcoat
dress shirt,tie
suit jacket,sun glasses
sun glasses,overcoat
left sock,pants
pants,belt
suit jacket,left shoe
suit jacket,right shoe
left shoe,overcoat
right sock,pants
right shoe,overcoat
t-shirt,suit jacket

### with values in quotes
"t-shirt","dress shirt"
"dress shirt","pants"
"dress shirt","suit jacket"
"tie","suit jacket"
"pants","suit jacket"
"belt","suit jacket"
"suit jacket","overcoat"
"dress shirt","tie"
"suit jacket","sun glasses"
"sun glasses","overcoat"
"left sock","pants"
"pants","belt"
"suit jacket","left shoe"
"suit jacket","right shoe"
"left shoe","overcoat"
"right sock","pants"
"right shoe","overcoat"
"t-shirt","suit jacket"


## Validation
The application will validate for:

- if filePath exists
- if filePath extension is either .csv or .txt
- if hasHeader flag is true, then check for existence of the column names Dependency and Item
- if rows in the file has both an dependency and item value

Validation failures will stop execution and display error messages.
