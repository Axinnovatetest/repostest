#  Pull Request - Backend (.NET 8)

##  Description
- **Ticket / Task :** #
- **Nature du changement :** [ex: Nouvel Endpoint API / Migration SQL]
- **Résumé :** (Décrire l'impact sur l'API ou la Base de données)

##  Checklist Technique
- [ ] **Validation :** Les entrées API sont validées (DTO, FluentValidation).
- [ ] **Base de données :** Les scripts SQL ou migrations sont inclus.
- [ ] **Performance :** Les requêtes LINQ/SQL sont optimisées (pas de boucle inutile).
- [ ] **Swagger :** Les nouveaux endpoints sont documentés et testables.

##  Documentation & Suivi
- [ ] **README.md :** Mis à jour si une nouvelle variable d'environnement (`appsettings.json`) est requise.
- [ ] **RELEASE.md :** Nouvelle ligne ajoutée avec la version et l'auteur.

##  Tests
- [ ] J'ai testé l'endpoint sur **Postman / Swagger** et il retourne un code 200/201.

