# PublicationsAnnualReport

Application for bibliometric analyses.

Originally written in order to facilitate the production of the bibliometric tables and lists required for the university annual report. Subsequently various other tasks have been piggybacked on top, using the same basic bibliometry framework.

Bibliometric primary input data can be in DIVA or RIS format. Button: "Läs CAS- och DIVA-fil" or "Läs RIS-filer"

A list of authors of interest is also required, as a text file with tab-separated fields. Button: "Läs CAS- och DIVA-fil"

Data from Scopus and/or Web of Science citation analysis can be connected with the primary data. Buttons: "Matcha SCOPUS" or "Läs KTH-fil".

Publication status from "Norska listan" and/or JCR can be connected with the primary data. Button "Tidskriftsdata"

Output can be produced in various formats for various purposes:
- Publication summary tables for annual report. Button "Tabell till årsredovisningen"
- Publication lists in APA format, as Word files. Button "Tabell till årsredovisningen", check "Skapa word-filer"
- Publication lists in Excel, with DIVA data grouped according to author affiliation.
- Citation statistics. Button: "Citeringsstatistik"
- Most cited authors in each unit (institution, department, ...). Button: "Toppförfattare"
- Word cloud images. Button: "Skapa ordmoln"
- Input data for collaboration graph production with Gephi or R. Button: "Läs RIS-filer", check "Skapa Gephi-filer" or "Skapa CSV-filer till R"
