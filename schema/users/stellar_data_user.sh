#!/bin/bash
set -e

# Check if password argument is provided
if [ $# -eq 0 ]; then
    echo "Error: Password required as first argument"
    echo "Usage: $0 <password>"
    exit 1
fi
# Configuration
DB_NAME="stellar_data"
DB_USER="stellar_data"
DB_PASSWORD="$1"
DB_ADMIN_USER="trasa"

# Check if we can connect to PostgreSQL
if ! psql -U $DB_ADMIN_USER postgres -c '\q' 2>/dev/null; then
    echo "Error: Cannot connect to PostgreSQL as user '$DB_ADMIN_USER'."
    echo "Please ensure PostgreSQL is running and you have proper authentication."
    exit 1
fi

# Create user and grant privileges
if psql -U $DB_ADMIN_USER postgres <<EOF
-- Create the user if it doesn't exist
DO \$\$
BEGIN
  IF NOT EXISTS (SELECT FROM pg_catalog.pg_user WHERE usename = '$DB_USER') THEN
    CREATE USER $DB_USER WITH PASSWORD '$DB_PASSWORD';
  END IF;
END
\$\$;

-- Grant connection privilege to the database
GRANT CONNECT ON DATABASE $DB_NAME TO $DB_USER;

-- Connect to the database and grant privileges
\c $DB_NAME

-- Grant usage on schema
GRANT USAGE ON SCHEMA public TO $DB_USER;

-- Grant privileges on all existing tables
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO $DB_USER;

-- Grant privileges on all existing sequences
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO $DB_USER;

-- Grant privileges on future tables (this is key for tables not yet created)
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO $DB_USER;

-- Grant privileges on future sequences
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT USAGE, SELECT ON SEQUENCES TO $DB_USER;

-- If you want the user to be able to create tables/objects
GRANT CREATE ON SCHEMA public TO $DB_USER;

EOF
then
  echo "User '$DB_USER' created and privileges granted successfully!"
  exit 0
else
  echo "ERROR: Failed to create user or grant privileges: error code $?"
  exit 1
fi
