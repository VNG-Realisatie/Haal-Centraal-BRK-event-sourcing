# Haal Centraal BRK events

## Let op! Deze API wordt tot nader order niet door het Kadaster aangeboden!

![lint oas](https://github.com/VNG-Realisatie/Haal-Centraal-BRK-event-sourcing/workflows/lint-oas/badge.svg)

De WOZ- en Erfpachtregistratie zijn grotendeels gebaseerd op de basisregistratie kadaster (BRK). Bijna iedere verandering van een kadastrale onroerende zaak leidt tot een verandering in de WOZ- en erfpachtregistratie. Daarom halen gemeenten wijzigingen op bij het kadaster en leggen een lokale kopie met historie aan. Dat kost veel energie, tijd en geld, en gaat niet altijd goed. Het kan niet anders, omdat gemeenten met terugwerkende kracht correcties moeten doen. Bijvoorbeeld n.a.v. een bezwaar. 

Door alle BRK gebeurtenissen op te slaan als onveranderlijke opeenvolgende feiten of EVENTS, net als de financiële feiten in een grootboek, kunnen gemeenten met een Publish Subscribe API worden genotificeerd op het moment dat zo’n event plaatsvindt, en met een REST API deze events opzoeken en raadplegen. Voordelen van een centrale event log:
* de lokale kopie is niet meer nodig;
* “loose coupling” tussen de aanbieder en abonnees, omdat het leveren en bevragen van events onafhankelijk van elkaar gebeurt;
* tijdreizen wordt gemakkelijk. Handig voor het ophalen en opnieuw afspelen van kadasterevents na herziening van WOZ object in het verleden. Zo kun je het object na correctie opnieuw bijwerken tot de actuele status;
* uitbesteden aan een belastingsamenwerking en wisselen van softwareleverancier wordt gemakkelijker: de nieuwe partij hoeft alleen de BRK (en BAG) events van de gemeente vanaf een bepaald tijdstip op te halen om nieuwe WOZ objecten voor de gemeente te kunnen produceren.

## Direct mee experimenteren?
* Bekijk de Events Bevragen API specificaties met [Swagger UI](https://petstore.swagger.io/?url=https://raw.githubusercontent.com/VNG-Realisatie/Haal-Centraal-BRK-event-sourcing/Check-op-links-en-teksten/specificatie/genereervariant/openapi.yaml).
* Bekijk de Notificaties API specificaties met [AsyncAPI playground](https://playground.asyncapi.io/?load=https://raw.githubusercontent.com/VNG-Realisatie/Haal-Centraal-BRK-event-sourcing/master/specificatie/asyncapi.yaml) 

## Heb je meer nodig? 
Gebruik de API in combinatie met de andere BRK API’s:
* [Actuele BRK-gegevens bevragen](https://vng-realisatie.github.io/Haal-Centraal-BRK-bevragen/)

## Bronnen
* [Productvisie Haal Centraal](https://vng-realisatie.github.io/Haal-Centraal)
* [API Design Visie](https://github.com/Geonovum/KP-APIs/tree/master/Werkgroep%20Design%20Visie)
* [REST API Design Rules](https://docs.geostandaarden.nl/api/API-Designrules/)
* [Landelijke API strategie voor de overheid](https://geonovum.github.io/KP-APIs/)

## Contact
* Product Owner: Cathy Dingemanse, [c.dingemanse@comites.nl](mailto:c.dingemanse@comites.nl)
* Designer & Customer zero: Melvin Lee, [melvin.lee@iswish.nl](mailto:melvin.lee@iswish.nl)

## Licentie
Copyright &copy; VNG Realisatie 2020
Licensed under the [EUPL](https://github.com/VNG-Realisatie/Haal-Centraal-BRP-bevragen/blob/master/LICENCE.md)
