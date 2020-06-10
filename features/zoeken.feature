#language: nl

Functionaliteit: Kadastraal onroerende zaak events zoeken / ophalen
    # event moet de volgdende properties bevatten
    # identificatie = topic id + partition id + offset van event
    # tijdstip = creatie tijdstip van event
    # aard stukdeel, datum

Scenario: nieuwe events van een abonnee
    Als de consumer de request 'GET /abonnement/kadastraalonroerendezaakevents' stuurt naar de events API
    Dan bevat de response de nog niet opgehaalde kadastraal onroerende zaak events van alle geabonneerde topics van de abonnee
    # abonnee wordt bepaald aan de hand van apikey / oAuth identity
    # tot een max aantal

Scenario: nieuwe events binnen een topic van een abonnee
    Als de consumer de request 'GET /abonnement/kadastraalonroerendezaakevents?topics=0344' stuurt naar de events API
    Dan bevat de response de nog niet opgehaalde kadastraal onroerende zaak events van topic '0344' van de abonnee
    # abonnee wordt bepaald aan de hand van apikey / oAuth identity
    # tot een max aantal

Scenario: nieuwe events binnen meerdere topics van een abonnee
    Als de consumer de request 'GET /abonnement/kadastraalonroerendezaakevents?topics=0344,0518' stuurt naar de events API
    Dan bevat de response de nog niet opgehaalde kadastraal onroerende zaak events van topic '0344' van de abonnee
    # abonnee wordt bepaald aan de hand van apikey / oAuth identity
    # tot een max aantal

Scenario: met kadastraal onroerende zaak identificatie
    Als de consumer de request 'GET /kadastraalonroerendezaakevents?kadastraalonroerendezaakidentificatie=123456' stuurt naar de events API
    Dan bevat de response alle kadastraal onroerende zaak events gekoppeld aan kadastraalonroerendezaakidentificatie '123456'

Scenario: met stuk identificatie
    Als de consumer de request 'GET /kadastraalonroerendezakenevents?stukidentificatie=123456' stuurt naar de events API
    Dan bevat de response alle kadastraal onroerende zaak events gekoppeld aan stukidentificatie '123456'

Scenario: met zakelijk gerechtigde identificatie
    Als de consumer de request 'GET /kadastraalonroerendezaakevents?zakelijkgerechtigdeidentificatie=123456' stuurt naar de events API
    Dan bevat de response alle kadastraal onroerende zaak events gekoppeld aan zakelijkgerechtigdeidentificatie '123456'

Scenario: met kadastraal onroerende zaak identificatie en vanaf een tijdstip
    Als de consumer de request 'GET /kadastraalonroerendezaakevents?kadastraalonroerendezaakidentificatie=123456&tijdstipvan=2020-01-01T00:00:00'
    Dan bevat de response de kadastraal onroerende zaak events gekoppeld aan kadastraalonroerendezaakidentificati 123456 en gecreerd vanaf 2020-01-01T00:00:00

Scenario: met kadastraal onroerende zaak identificatie en vanaf en tot een tijdstip
    Als de consumer de request 'GET /kadastraalonroerendezaakevents?tijdstipvan=2020-01-01T00:00:00&tijdstiptot=2020-02-01T00:00:00'
    Dan bevat de response de kadastraal onroerende zaak events gekoppeld aan kadastraalonroerendezaakidentificatie 123456 en vanaf 2020-01-01T00:00:00 tot 2020-02-01T00:00:00

Scenario: met event identificatie
    Gegeven er is een kadastraal onroerende zaak event met identificatie 123456789
    Als de consumer de request 'GET /kadastraalonroerendezaakevents/123456789'
    Dan bevat de response de kadastraal onroerende zaak event met identificatie 123456789
