graph TD
    A[Utilisateur crée une commande] --> B{Vérification Stock}
    B -- En Stock --> C[Validation Automatique]
    B -- Hors Stock --> D[Alerte Logistique]
    
    C --> E[Envoi vers API .NET]
    D --> F[Attente Réapprovisionnement]
    
    E --> G{Statut Réponse}
    G -- Succès --> H[Mise à jour Dashboard Sales]
    G -- Erreur --> I[Log Erreur & Alerte Dev]

    subgraph "Équipe Technique"
    I
    end
    
    subgraph "Utilisateurs Finaux"
    A
    H
    end
