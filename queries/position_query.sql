SELECT system_name, 
    galactic_x, galactic_y, galactic_z,
    distance_sol_ly, 
    ST_3DDistance(position, ST_MakePoint(0, 0, 0)) as calculated_distance
FROM star_systems
WHERE ST_3DDWithin(position, ST_MakePoint(0, 0, 0), 10)
ORDER BY calculated_distance

