---
layout: page
title: BRK Event Sourcing
---
# BRK Event Sourcing

De WOZ- en Erfpachtregistratie zijn voor een belangrijk deel gebaseerd op de basisregistratie kadaster. Iedere verandering van een kadastrale onroerende zaak leidt tot een verandering in beide registraties. Nu halen gemeenten wijzigen op die zij in een lokale kopie verwerken om zelf historie op te bouwen. Dat kost veel energie, tijd en geld, en gaat niet altijd goed. 

Door alle gebeurtenissen in de Basisregistratie Kadaster op te slaan in de vorm van onveranderlijke opeenvolgende feiten of EVENTS, net als de financiële feiten in een grootboek, dan kunnen gemeenten met een Publish Subscribe API worden genotificeerd op het moment dat zo’n event plaatsvindt, en met een REST API deze events opzoeken en raadplegen.

Voordelen van een centrale event log:

1. de event log zorgt voor “loose coupling” tussen de aanbieder en zijn abonnees, omdat het leveren en bevragen van events onafhankelijk van elkaar gebeurt;

2. de event log maakt tijdreizen gemakkelijk. Handig voor het ophalen en opnieuw afspelen van kadasterevents na herziening van WOZ object in het verleden. Zo kan het gecorrigeerde object opnieuw worden bijgewerkt;

3. uitbesteden aan een belastingsamenwerking en wisselen van softwareleverancier wordt gemakkelijker: de nieuwe partij hoeft alleen de BRK (en BAG) events van de gemeente vanaf een bepaald tijdstip op te halen om nieuwe WOZ objecten voor de gemeente te kunnen produceren.

De event store als de “single source of truth” is onmisbaar om lokale kopieën op te ruimen en de common ground agenda te realiseren!
