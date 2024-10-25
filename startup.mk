.PHONY: workschedule

workschedule:
	cd ./ComposeFiles && docker compose up -d database database
	cd ./ComposeFiles && docker compose up -d backend backend --build backend