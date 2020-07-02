# BRK event sourcing POC

Met deze POC is gekeken of [Apache Kafka](https://kafka.apache.org/) als basis kan worden gebruikt om de [Event Sourcing](https://martinfowler.com/eaaDev/EventSourcing.html) pattern te implementeren.

Voor de POC zijn twee volledig functionerende API's geïmplementeerd:

- KadastraleOnroerendeZaken API. Met deze API kunnen Kadastrale Onroerende Zaken en Zakelijk Gerechtigden worden aangemaakt en gewijzigd. De bijbehorende state wijzigingen worden in Kafka gepubliceerd.
- KadastraleOnroerendeZakenEvents API. Met deze API kunnen de in Kafka gepubliceerde state wijzigingen worden geconsumeerd.

De OAS specificaties van de API's zijn afgeleid van de [BRK-Bevragen](https://raw.githubusercontent.com/VNG-Realisatie/Haal-Centraal-BRK-bevragen/master/specificatie/BRK-Bevragen/genereervariant/openapi.yaml) en [BRK events bevragen](https://raw.githubusercontent.com/VNG-Realisatie/Haal-Centraal-BRK-event-sourcing/master/specificatie/genereervariant/openapi.yaml) API specificaties en aangepast ten behoeve van de POC.
