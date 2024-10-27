.PHONY: database-update

database-update:
	cd ./ComposeFiles && docker compose -f database-update.yaml up -d database database
	cd ./ComposeFiles && docker compose -f database-update.yaml up --abort-on-container-exit --exit-code-from backend-db-update backend-db-update --build backend-db-update
	cd ./ComposeFiles && docker compose -f database-update.yaml down backend-db-update