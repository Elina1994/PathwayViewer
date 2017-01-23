### README for the application PathwayViewer

##### What is this repository for ?
This repository contains the functionality for the application PathwayViewer.

##### Used Software
- C#
- Python 3.5
- MUSCLE

##### How do I get set up ?
For correctly using this application, additional files are needed.
These files contain several datasets on which this application depends.
The files are stored in a zip archive and need to be unpacked before using the application.

The exact functionality of this application is described in the user manual. This user
manual will be delivered including the zip archive. 

##### Who do I talk to ?
Elina : elinastellark@gmail.com

#### Application details
This application has several functions. When following every step of the application it
generates a visualisation of pathways of micro-organisms and the taxonomy of these micro-organisms.

###### Data collection
This is the first section of the application, it depends mainly on Python scripts. These python scripts can
be found at: https://github.com/Elina1994/DataCollector . Data is collected and saved from NCBI and KEGG.

###### Data mapping
The data collected from NCBI and KEGG is mapped to create an overlap between the datasets. This is needed in order to create
a suitable visualisation.

###### Data visualisation
This is the final step from the application. It uses the data from the previous mapping step to generate a visualisation.
The product of this visualisation is a phylogenetic tree and a pie chart.

Usage details can be found in the delivered manual.
