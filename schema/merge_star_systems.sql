CREATE OR REPLACE FUNCTION merge_star_systems(
    p_star_ids INTEGER[],
    p_correct_system_id INTEGER
) returns TABLE (updated_stars INTEGER, deleted_systems INTEGER) AS $$
DECLARE
    v_duplicate_system_ids INTEGER[];
    v_updated_count INTEGER;
    v_deleted_count INTEGER;
BEGIN
    -- get duplicate system ids
    SELECT ARRAY_AGG(DISTINCT star_system_id)
    INTO v_duplicate_system_ids
    FROM stars
    WHERE id = ANY(p_star_ids) AND star_system_id != p_correct_system_id;

    RAISE NOTICE 'input stars: %', p_star_ids;
    RAISE NOTICE 'correct system id: %', p_correct_system_id;
    RAISE NOTICE 'duplicate system ids: %', v_duplicate_system_ids;

    -- update stars
    UPDATE stars set star_system_id = p_correct_system_id
    WHERE id = ANY(p_star_ids);

    GET DIAGNOSTICS v_updated_count = ROW_COUNT;

    -- delete duplicate systems
    DELETE FROM star_systems
    WHERE id = ANY(v_duplicate_system_ids);

    GET DIAGNOSTICS v_deleted_count = ROW_COUNT;

    return QUERY SELECT v_updated_count, v_deleted_count;
END;
$$ LANGUAGE plpgsql;
