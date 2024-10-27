.PHONY: db-startup

db-startup:
	cd ./ComposeFiles && docker compose up -d database database