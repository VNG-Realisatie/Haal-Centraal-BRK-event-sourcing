---
layout: page-with-side-nav
title: BRK Event Sourcing
---
# BRK Event Sourcing

![lint oas](https://github.com/VNG-Realisatie/Haal-Centraal-BRK-event-sourcing/workflows/lint-oas/badge.svg)

De WOZ- en Erfpachtregistratie zijn grotendeels gebaseerd op de basisregistratie kadaster. Bijna iedere verandering van een kadastrale onroerende zaak leidt tot een verandering in de basisregistratie WOZ. Gemeenten halen daarvoor wijzigingen bij het kadaster op leggen daarmee een lokale kopie met historie aan. Dat kost veel energie, tijd en geld, en gaat niet altijd goed. 

Door alle gebeurtenissen in de Basisregistratie Kadaster op te slaan in de vorm van onveranderlijke opeenvolgende feiten of EVENTS, net als de financiële feiten in een grootboek, kunnen gemeenten met een Publish Subscribe API worden genotificeerd op het moment dat zo’n event plaatsvindt, en met een REST API deze events opzoeken en raadplegen. Voordelen van een centrale event log:
1. de event log zorgt voor “loose coupling” tussen de aanbieder en zijn abonnees, omdat het leveren en bevragen van events onafhankelijk van elkaar gebeurt;
2. de event log maakt tijdreizen gemakkelijk. Handig voor het ophalen en opnieuw afspelen van kadasterevents na herziening van WOZ object in het verleden. Zo kan het gecorrigeerde object opnieuw worden bijgewerkt;
3. uitbesteden aan een belastingsamenwerking en wisselen van softwareleverancier wordt gemakkelijker: de nieuwe partij hoeft alleen de BRK (en BAG) events van de gemeente vanaf een bepaald tijdstip op te halen om nieuwe WOZ objecten voor de gemeente te kunnen produceren.

## Direct aan de slag?
* Bekijk de Events Bevragen API specificaties met [Swagger UI](https://vng-realisatie.github.io/Haal-Centraal-BRK-event-sourcing/swagger-ui) of [Redoc](https://vng-realisatie.github.io/Haal-Centraal-BRK-event-sourcing/redoc)
* Bekijk de Notificaties API specificaties met [AsyncAPI playground](https://playground.asyncapi.io/?load=https://raw.githubusercontent.com/VNG-Realisatie/Haal-Centraal-BRK-event-sourcing/master/specificatie/asyncapi.yaml) 

## Heb je meer nodig? 
Gebruik de API in combinatie met de andere API’s:
* [Actuele BRK-gegevens bevragen](https://vng-realisatie.github.io/Haal-Centraal-BRK-bevragen/){:target="_blank" rel="noopener"}

## Bronnen

* [Productvisie Haal Centraal](https://vng-realisatie.github.io/Haal-Centraal){:target="_blank" rel="noopener"}
* [API Design Visie](https://github.com/Geonovum/KP-APIs/tree/master/Werkgroep%20Design%20Visie){:target="_blank" rel="noopener"}
* [REST API Design Rules](https://docs.geostandaarden.nl/api/API-Designrules/){:target="_blank" rel="noopener"}
* [Landelijke API strategie voor de overheid](https://geonovum.github.io/KP-APIs/){:target="_blank" rel="noopener"}

## Licentie

Copyright &copy; VNG Realisatie 2018
Licensed under the [EUPL](https://github.com/VNG-Realisatie/Haal-Centraal-BRP-bevragen/blob/master/LICENCE.md){:target="_blank" rel="noopener"}

