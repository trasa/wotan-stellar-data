
-- Find the 10 nearest stars to a specific coordinate
SELECT 
    system_name,
    galactic_x,
    galactic_y,
    galactic_z,
    ST_3DDistance(position, ST_MakePoint(5, 10, 2)) as distance
FROM star_systems
ORDER BY position <-> ST_MakePoint(5, 10, 2)  -- Uses spatial index!
LIMIT 10;
