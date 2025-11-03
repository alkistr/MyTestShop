#!/bin/bash

# Docker Management Script for MyTestShop SQL Server

case "$1" in
    start)
        echo "Starting SQL Server container..."
        sudo docker-compose up -d
        echo "Waiting for SQL Server to be ready..."
        sleep 10
        sudo docker logs mytestshop-sqlserver --tail 5
        ;;
    stop)
        echo "Stopping SQL Server container..."
        sudo docker-compose down
        ;;
    restart)
        echo "Restarting SQL Server container..."
        sudo docker-compose restart
        ;;
    logs)
        echo "Showing SQL Server logs..."
        sudo docker logs mytestshop-sqlserver --follow
        ;;
    status)
        echo "SQL Server container status:"
        sudo docker ps --filter "name=mytestshop-sqlserver" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
        ;;
    migrate)
        echo "Running database migrations..."
        cd MyTestShop
        dotnet ef database update --project ../MyTestShop.Infrastructure --startup-project .
        cd ..
        ;;
    connect)
        echo "Connecting to SQL Server..."
        sudo docker exec -it mytestshop-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Your_password123"
        ;;
    *)
        echo "Usage: $0 {start|stop|restart|logs|status|migrate|connect}"
        echo ""
        echo "Commands:"
        echo "  start    - Start the SQL Server container"
        echo "  stop     - Stop the SQL Server container"
        echo "  restart  - Restart the SQL Server container"
        echo "  logs     - Show SQL Server logs (follow mode)"
        echo "  status   - Show container status"
        echo "  migrate  - Run database migrations"
        echo "  connect  - Connect to SQL Server using sqlcmd"
        exit 1
        ;;
esac