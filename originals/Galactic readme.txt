GALACTIC CO-ORDINATE RECONS, DENSE, and CTIOPI DATASETS
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

+ + + + These datasets are NOT for use with Astrosynthesis! + + + + 

--RECONS dataset--  (Jan 2012 version)
~~~~~~~~~~~~~~~~~~

RECONS is currently the most accurate list of over 100 star systems within 22.7 lightyears of Sol, and is updated at least once a year. 

GAL_RECONS.csv - Contains RECONS near star data in Galactic XYZ format. (+X = Coreward, +Y = Spinward, +Z = "up"/Galactic North). 
Usable for any purpose EXCEPT Astrosynthesis! 
Format: Star, ID#, Star Name, Galactic X (ly), Galactic Y (ly), Galactic Z (ly), mass, (blank), (blank), Spectral Type/Size, (blank), distance (ly).

Creation date: Jan 2012, by Constantine Thomas.
Updated: 5 Feb 2012 - addded Gliese 667 (HIP 84709, HIP star within RECONS sphere).

Source: http://www.recons.org/TOP100.posted.htm


+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

--DENSE dataset--
~~~~~~~~~~~~~~~~~

DENSE is a catalogue of over 100 white dwarfs located within 25pc of Sol. 

Note: This is NOT the original dataset from http://www.denseproject.com/25pc/ - it has been edited for typo corrections in the original data, and also 
has had alternative star names added (where known). Spectral types have been added (these are accurate as far as I can tell) from the White Dwarf
Catalogue at http://www.astronomy.villanova.edu/WDCatalog/index.html . 

Also, some of the entries from the original DENSE list have been removed since there is some overlap in the CTIOPI dataset - full details can be found in the 
"CTIOPI-DENSE merging details" section below.

(NOTE: all DENSE entries within 22.7 lightyears have been removed to avoid overlap with RECONS).  

GAL_DENSE.csv - XYZ co-ordinates in Galactic XYZ format. (+X = Coreward, +Y = Spinward, +Z = "up"/Galactic North)
Usable for any purpose EXCEPT Astrosynthesis! 
Format: Star, ID#, Star Name, Galactic X (ly), Galactic Y (ly), Galactic Z (ly), mass (generic), radius (generic), luminosity (generic), Spectral Type/Size (accurate), (blank), distance (ly).


Creation date: Jan 2012, by Constantine Thomas.
Typos corrected: 26/2/12
0029-021 corrected to 0029-031 (source: http://www.astronomy.villanova.edu/WDCatalog/index.html)
Right Ascension for 0821-669 is wrong - corrected to 08h 21m 26.7s (source: Subasavage et al., 2009)

Source: http://www.denseproject.com/25pc/ (edited)


+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

--CTIOPI dataset--
~~~~~~~~~~~~~~~~~~

CTIOPI (Cerro Tololo Interamerican Observatory Parallax Investigation - http://www.chara.gsu.edu/~thenry/CTIOPI/index.htm ) is another 
dataset from the RECONS group - its purpose is to discover red, white, and brown dwarfs that are within 25pc of Sol. 

It is important to note that the dataset included here is NOT the same as the original CTIOPI dataset on the RECONS website (located at 
http://www.recons.org/publishedpi ). There is some overlap between the CTIOPI dataset and the DENSE white dwarf dataset described above, which
means that both datasets have been edited to remove duplicates. Full details can be found in the "CTIOPI-DENSE merging details" section below.

(NOTE: all CTIOPI entries within 22.7 lightyears have been removed to avoid overlap with RECONS)

GAL_CTIOPI.csv - XYZ co-ordinates in Galactic XYZ format. (+X = Coreward, +Y = Spinward, +Z = "up"/Galactic North)
Usable for any purpose EXCEPT Astrosynthesis! 
Format: Star, ID#, Star Name, Galactic X (ly), Galactic Y (ly), Galactic Z (ly), mass (generic), radius (generic), luminosity (generic), Spectral Type/Size (accurate), (blank), distance (ly).


Creation date: Feb 2012, by Constantine Thomas.
First posted: 26/2/12.
Updated: 24th March 2010. Separated multiple-star systems have now been re-combined - originally A/B/C components of some multiple-star systems were separated by several lightyears due to 
parallax inconsistencies. If A and B components were separated, they have now been combined as Multiple systems in the data (the A star is assumed to have the correct distance for the system). 
Some stars that are listed as "A" components without any B components nearby remain as they were - these have been left as single stars, but you can assume that they are actually multiple stars 
with the other components at the same location as the A star. 

Source: http://www.recons.org/publishedpi (edited)


+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

--CTIOPI/DENSE Merging details--
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The CTIOPI Dataset included some of the white dwarfs from the DENSE dataset (some with updated/different RA/Dec and/or Plx values), which meant
edits had to be made to both datasets in order to avoid duplication. 

NOTE: All of these updates were included in the initial 26/2/12 CTIOPI and DENSE datasets posted on the Stellar Mapping site.
 

Kept in DENSE, removed from CTIOPI (identical RA/Dec and plx in both datasets, CTIOPI data removed to prevent duplication):

WD 0141-675	LHS 145 (WD 0141-675) - updated in AS3_DENSE.csv, spectral types updated in DENSE, removed from CTIOPI
WD 0806-661	L093-003 (WD 0806-661) - updated in AS3_DENSE.csv, spectral types updated in DENSE, removed from CTIOPI
WD 1036-204	LHS 2293 (WD 1036-204) - updated in AS3_DENSE.csv, spectral types updated in DENSE, removed from CTIOPI
WD 1202-232	LP 852-007 (WD 1202-232) - updated in AS3_DENSE.csv, spectral types updated in DENSE, removed from CTIOPI
WD 1223-659	Gliese 2092 (WD 1223-659) - updated in AS3_DENSE.csv, spectral types updated in DENSE, removed from CTIOPI
WD 1315-781	L 040-116 (WD 1315-781) - updated in AS3_DENSE.csv, spectral types updated in DENSE, removed from CTIOPI
WD 1436-781	LTT 5814 (WD 1436-781) - updated in AS3_DENSE.csv, spectral types updated in DENSE, removed from CTIOPI
WD 2008-600	SCR 2012-5956 (WD 2008-600) - updated in AS3_DENSE.csv, spectral types updated in DENSE, removed from CTIOPI
WD 2008-799	SCR 2016-7945 (WD 2008-799) - updated in AS3_DENSE.csv, spectral types updated in DENSE, removed from CTIOPI
WD 2040-392	L495-082 (WD 2040-392) - updated in AS3_DENSE.csv, spectral types updated in DENSE, removed from CTIOPI
WD 2138-332	P3327-2 (WD 2138-332) - updated in AS3_DENSE.csv, spectral types updated in DENSE, removed from CTIOPI
WD 2336-079	GD 1212 (WD 2336-079) - updated in AS3_DENSE.csv, spectral types updated in DENSE, removed from CTIOPI

-------------

Kept in DENSE, removed from CTIOPI (DENSE plx is more accurate):

WD 0751-252 (SCR 0753-2524) - kept in DENSE, removed from CTIOPI
WD 1009-184 (WT 1759) - kept in DENSE, removed from CTIOPI

-------------

Removed from DENSE, kept in CTIOPI (CTIOPI plx is more accurate):

WD 0038-226 - Removed from DENSE, kept in CTIOPI
WD 0738-172 - Removed from DENSE, kept in CTIOPI
WD 0752-676 - Removed from DENSE, kept in CTIOPI
WD 0821-669 - Removed from DENSE, kept in CTIOPI, updated XYZ based on correct Right Ascension.
WD 0839-327 - Removed from DENSE, kept in CTIOPI
WD 2251-070 - Removed from DENSE, kept in CTIOPI

-------------

Removed from DENSE (WDs already present in HIP catalogue. Assumed that HIP plx is more accurate than weighted means in DENSE table)

WD 0148+467 - HIP 8709 (GJ 3121)
WD 0208-510 - HIP 10138
WD 0310-688 - HIP 14754 (GJ 127.1 A)
WD 0644+375 - HIP 32560 (GJ 246/LHS 1870)
WD 1132-325 - HIP 56452 (incorrectly named in HIP as "20 Crateris", actually Gliese 432B)
WD 1134+300 - HIP 56662 (GJ 433.1)
WD 1327-083 - HIP 65877 (GJ 515/Wolf 485A)
WD 1544-377 - HIP 77358 (GJ 599B)
WD 1620-391 - HIP 80300 (GJ 620.1 B)
WD 1647+591 - HIP 82257 (DN Draconis)
WD 1917-077 - HIP 95071 (GJ 754.1 A)
WD 2007-303 - HIP 99438 (GJ 2147)
WD 2032+248 - HIP 101516 (Wolf 1346)
WD 2039-202 - HIP 102207 (GJ 799.1)
WD 2253+054 - HIP 113244 (GJ 4305B)
WD 2341+322 - HIP 117059 (GJ 905.2 B)

WD 0046+051 - already removed (in RECONS)
WD 0426+588 - already removed (in RECONS)
WD 1142-645 - already removed (in RECONS)

-------------

Only present in CTIOPI:

LHS 193A (WD 0430-391)
Gliese 915 (WD 2359-434)
LHS 4040 (WD 2331-335)
L 064-040 (WD 0928-713)
Gliese 781.3 (WD 2007-219)
LHS 3245 (WD 1647-327)

-------------

Other CTIOPI Updates:

LHS 193A plx averaged with LHS 193B (now updated to 0.03387 in CTIOPI.csv and furtherstars.xlsx)
LHS 4039 plx averaged with LHS 4040 (now updated to 0.04353 in CTIOPI.csv)
